using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class ServicesModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public ServicesModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public List<Service> Services { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string FilterCategory { get; set; } = "Tất cả";

        public async Task OnGetAsync()
        {
            var allServices = await _serviceService.GetAvailableServicesAsync();

            Services = FilterCategory switch
            {
                "Tiết kiệm" => allServices.Where(s => s.Price <= 500000).ToList(),
                "Phổ thông" => allServices.Where(s => s.Price > 500000 && s.Price <= 1000000).ToList(),
                "Cao cấp" => allServices.Where(s => s.Price > 1000000).ToList(),
                _ => allServices
            };
        }

    }
}
