using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class CreateModel : PageModel
    {
        private readonly Data.Models.GenderHealthcareContext _context;

        public CreateModel(Data.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Clinics.Add(Clinic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
