using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;
using System.Text.Json;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class ConfirmBookingTestingModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;

        public ConfirmBookingTestingModel(IUserService userService, IServiceService serviceService)
        {
            _userService = userService;
            _serviceService = serviceService;
        }

        public User? User { get; set; }
        public List<string> SelectedServiceNames { get; set; } = new();
        public DateTime AppointmentTime { get; set; }
        public bool IsLoggedIn { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                IsLoggedIn = true;
                User = await _userService.GetUserById(userId.Value);
            }
            else
            {
                IsLoggedIn = false;
            }

            // Lấy danh sách dịch vụ từ session
            var serviceIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            if (!string.IsNullOrEmpty(serviceIdsRaw))
            {
                var serviceIds = serviceIdsRaw.Split(',').Select(int.Parse).ToList();
                var allServices = await _serviceService.GetAllAsync();
                SelectedServiceNames = allServices
                    .Where(s => serviceIds.Contains(s.ServiceId))
                    .Select(s => s.Name)
                    .ToList();
            }

            // Lấy thời gian xét nghiệm
            var appointmentTimeRaw = HttpContext.Session.GetString("AppointmentTime");
            if (!string.IsNullOrEmpty(appointmentTimeRaw))
            {
                AppointmentTime = DateTime.Parse(appointmentTimeRaw);
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var serviceIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            var appointmentTimeRaw = HttpContext.Session.GetString("AppointmentTime");

            // Đảm bảo dữ liệu vẫn còn hợp lệ trong session
            if (string.IsNullOrEmpty(serviceIdsRaw) || string.IsNullOrEmpty(appointmentTimeRaw))
            {
                TempData["Error"] = "Thông tin đặt lịch không hợp lệ. Vui lòng thực hiện lại.";
                return RedirectToPage("SelectService");
            }

            // Gọi lại Set để giữ session sau redirect
            HttpContext.Session.SetString("SelectedServiceIds", serviceIdsRaw);
            HttpContext.Session.SetString("AppointmentTime", appointmentTimeRaw);

            return RedirectToPage("BookingTestingSuccess");
        }

    }
}
