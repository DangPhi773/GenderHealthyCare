using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Services.Interfaces; // 👈 Import interface

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

        public async Task OnGetAsync()
        {
            Service = await _serviceService.GetAllAsync();
        }
    }
}
