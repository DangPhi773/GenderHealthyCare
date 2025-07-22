using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc; // 👈 Import interface

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class IndexModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public IndexModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public IList<Service> Service { get; set; } = new List<Service>();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }

            Service = await _serviceService.GetAllAsync();
            return Page();

        }
    }
}
