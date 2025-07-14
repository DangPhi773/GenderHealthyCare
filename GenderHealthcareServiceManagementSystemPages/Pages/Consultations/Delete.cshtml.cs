using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Consultations
{
    public class DeleteModel : PageModel
    {
        private readonly BusinessObjects.Models.GenderHealthcareContext _context;

        public DeleteModel(BusinessObjects.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Consultation Consultation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations.FirstOrDefaultAsync(m => m.ConsultationId == id);

            if (consultation == null)
            {
                return NotFound();
            }
            else
            {
                Consultation = consultation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                Consultation = consultation;
                _context.Consultations.Remove(Consultation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
