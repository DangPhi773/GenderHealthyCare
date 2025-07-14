using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class IndexModel : PageModel
    {
        private readonly IClinicService _iClinicService;

        public IndexModel(IClinicService iClinicService)
        {
            _iClinicService = iClinicService;
        }

        [BindProperty(SupportsGet = true)]
        public string? ClinicName { get; set; }
        public IList<Clinic> Clinic { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (String.IsNullOrEmpty(ClinicName))
            {
                Clinic = await _iClinicService.GetAllAsync();
            }
            else
            {
                Clinic = await _iClinicService.GetClinicsByClinicName(ClinicName);
            }
            return Page();
        }
    }
}
