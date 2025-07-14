using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class BookingTestingSuccessModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;
        private readonly ITestService _testService;

        public BookingTestingSuccessModel(
            IUserService userService,
            IServiceService serviceService,
            ITestService testService)
        {
            _userService = userService;
            _serviceService = serviceService;
            _testService = testService;
        }

        public User? User { get; set; }
        public List<string> SelectedServiceNames { get; set; } = new();
        public DateTime AppointmentTime { get; set; }
        public bool IsLoggedIn { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            int? userId = null;

            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int parsedUserId))
            {
                userId = parsedUserId;
            }
            if (userId.HasValue)
            {
                IsLoggedIn = true;
                User = await _userService.GetUserById(userId.Value);
            }
            //else
            //{
            //    IsLoggedIn = false;
            //    return RedirectToPage("SelectService");
            //}

            var selectedIdsRaw = HttpContext.Session.GetString("SelectedServiceIds");
            if (!string.IsNullOrEmpty(selectedIdsRaw))
            {
                var serviceIds = selectedIdsRaw.Split(',').Select(int.Parse).ToList();

                var allServices = await _serviceService.GetAllAsync();

                var selectedServices = allServices
                    .Where(s => serviceIds.Contains(s.ServiceId))
                    .ToList();

                SelectedServiceNames = selectedServices.Select(s => s.Name).ToList();

                var appointmentRaw = HttpContext.Session.GetString("AppointmentTime");
                if (!string.IsNullOrEmpty(appointmentRaw))
                {
                    AppointmentTime = DateTime.Parse(appointmentRaw);
                }

                foreach (var service in selectedServices)
                {
                    var test = new Test
                    {
                        UserId = userId.Value,
                        ServiceId = service.ServiceId,
                        AppointmentTime = AppointmentTime,
                        Status = "Pending"
                    };

                    await _testService.AddTest(test);
                }
            }

            return Page();
        }
    }
}
