using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public DetailsModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetByIdAsync(id.Value);

            Service = service;
            return Page();
        }
    }
}
