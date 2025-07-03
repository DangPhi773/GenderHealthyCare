using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly Data.Models.GenderHealthcareContext _context;

        public DetailsModel(Data.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        public Clinic Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _context.Clinics.FirstOrDefaultAsync(m => m.ClinicId == id);
            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                Clinic = clinic;
            }
            return Page();
        }
    }
}
