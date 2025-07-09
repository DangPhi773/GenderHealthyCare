using BusinessObjects.Models;
using global::Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class SelectServiceModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public SelectServiceModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public List<Service> Services { get; set; } = new();
        public bool IsLoggedIn { get; set; }

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                IsLoggedIn = false;
                return Page(); // Cho hiển thông báo chưa login
            }

            IsLoggedIn = true;
            Services = (await _serviceService.GetAllServices()).ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            if (SelectedServiceIds == null || !SelectedServiceIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một dịch vụ.");
                return Page();
            }

            HttpContext.Session.SetString("SelectedServiceIds", string.Join(",", SelectedServiceIds));
            return RedirectToPage("SelectTimeTesting");
        }

    }
}

