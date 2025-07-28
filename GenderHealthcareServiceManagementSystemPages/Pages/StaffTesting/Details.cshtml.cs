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
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.Models.GenderHealthcareContext _context;

        public DetailsModel(BusinessObjects.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

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
    }
}
