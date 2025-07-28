using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class SelectTimeTestingModel : PageModel
    {
        private readonly IServiceService _serviceService;
        private readonly ITestService _testService;

        public SelectTimeTestingModel(IServiceService serviceService, ITestService testService)
        {
            _serviceService = serviceService;
            _testService = testService;
        }

        [BindProperty]
        public DateTime AppointmentTime { get; set; }

        public List<string> SelectedServiceNames { get; set; } = new();
        public List<int> SelectedServiceIds { get; set; } = new();
        public bool IsLoggedIn { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            int? userId = null;

            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            if (userId == null)
            {
                IsLoggedIn = false;
                return Page();
            }

            IsLoggedIn = true;

            AppointmentTime = DateTime.Today.AddDays(1).AddHours(8);

            var selectedIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            if (string.IsNullOrEmpty(selectedIdsRaw))
            {
                return RedirectToPage("SelectService");
            }

            SelectedServiceIds = selectedIdsRaw.Split(',').Select(int.Parse).ToList();
            TempData["SelectedServiceIds"] = selectedIdsRaw; 

            var allServices = await _serviceService.GetAllAsync();
            SelectedServiceNames = allServices
                .Where(s => SelectedServiceIds.Contains(s.ServiceId))
                .Select(s => s.Name)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            if (AppointmentTime == default)
            {
                ModelState.AddModelError(nameof(AppointmentTime), "Vui lòng chọn thời gian xét nghiệm.");
                return await ReloadPageData();
            }

            if (AppointmentTime < DateTime.Today.AddDays(1))
            {
                ModelState.AddModelError(nameof(AppointmentTime), "Vui lòng chọn ngày hẹn bắt đầu từ ngày mai.");
                return await ReloadPageData();
            }

            // Kiểm tra trùng lịch
            bool hasConflict = await _testService.IsAppointmentTimeTestingConflict(userId, AppointmentTime);
            if (hasConflict)
            {
                ModelState.AddModelError(nameof(AppointmentTime), "Bạn đã có lịch xét nghiệm khác tại thời điểm này.");
                return await ReloadPageData();
            }

            HttpContext.Session.SetString("AppointmentTime", AppointmentTime.ToString("o"));
            return RedirectToPage("ConfirmBookingTesting");
        }

        private async Task<IActionResult> ReloadPageData()
        {
            var selectedIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            if (string.IsNullOrEmpty(selectedIdsRaw))
            {
                return RedirectToPage("SelectService");
            }

            SelectedServiceIds = selectedIdsRaw.Split(',').Select(int.Parse).ToList();

            var allServices = await _serviceService.GetAllAsync();
            SelectedServiceNames = allServices
                .Where(s => SelectedServiceIds.Contains(s.ServiceId))
                .Select(s => s.Name)
                .ToList();

            IsLoggedIn = true;
            return Page();
        }
    }
}
