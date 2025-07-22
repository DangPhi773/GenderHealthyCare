using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces; // 👈 cần thêm namespace này

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class EditModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public EditModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null )
            {
                return NotFound();
            }

            Service = service;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingService = await _serviceService.GetByIdAsync(Service.ServiceId);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Name = Service.Name;
            existingService.Description = Service.Description;
            existingService.Price = Service.Price;

            var success = await _serviceService.UpdateAsync(existingService);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Cập nhật thất bại. Vui lòng thử lại.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
