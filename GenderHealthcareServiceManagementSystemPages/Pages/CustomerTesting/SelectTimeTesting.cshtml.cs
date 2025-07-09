using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class SelectTimeTestingModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public SelectTimeTestingModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [BindProperty]
        public DateTime AppointmentTime { get; set; }

        public List<string> SelectedServiceNames { get; set; } = new();
        public List<int> SelectedServiceIds { get; set; } = new();
        public bool IsLoggedIn { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                IsLoggedIn = false;
                return Page();
            }

            IsLoggedIn = true;

            AppointmentTime = new DateTime(DateTime.Now.Year, 1, 1, 8, 0, 0);

            var selectedIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            if (string.IsNullOrEmpty(selectedIdsRaw))
            {
                return RedirectToPage("SelectService");
            }

            SelectedServiceIds = selectedIdsRaw.Split(',').Select(int.Parse).ToList();
            TempData["SelectedServiceIds"] = selectedIdsRaw; // Preserve for next page

            var allServices = await _serviceService.GetAllAsync();
            SelectedServiceNames = allServices
                .Where(s => SelectedServiceIds.Contains(s.ServiceId))
                .Select(s => s.Name)
                .ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //if (userId == null)
            //{
            //    return RedirectToPage("/Account/Login");
            //}

            if (AppointmentTime == default)
            {
                ModelState.AddModelError(nameof(AppointmentTime), "Vui lòng chọn thời gian xét nghiệm.");
                return Page();
            }

            HttpContext.Session.SetString("AppointmentTime", AppointmentTime.ToString("o")); // ISO format
            return RedirectToPage("ConfirmBookingTesting");
        }

    }
}
