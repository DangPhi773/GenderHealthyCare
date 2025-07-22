using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class CreateModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public CreateModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Service.IsDeleted = false;

            var success = await _serviceService.AddAsync(Service);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Không thể tạo dịch vụ. Vui lòng thử lại.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
