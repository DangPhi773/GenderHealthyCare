using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class DeleteModel : PageModel
    {
        private readonly BusinessObjects.Models.GenderHealthcareContext _context;

        public DeleteModel(BusinessObjects.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FirstOrDefaultAsync(m => m.TestId == id);

            if (test == null)
            {
                return NotFound();
            }
            else
            {
                Test = test;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            if (test != null)
            {
                Test = test;
                test.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
