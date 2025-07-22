using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class ClinicsModel : PageModel
    {
        private readonly IClinicService _clinicService;

        public ClinicsModel(IClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        public List<Clinic> Clinics { get; set; } = new();

        public async Task OnGetAsync()
        {
            Clinics = await _clinicService.GetAllAsync();
        }
    }
}
