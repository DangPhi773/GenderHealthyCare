using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Feedbacks
{
    public class CreateModel : PageModel
    {
        private readonly BusinessObjects.Models.GenderHealthcareContext _context;

        public CreateModel(BusinessObjects.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            ViewData["ConsultantId"] = new SelectList(_context.Users, "UserId", "Email");
        ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "Name");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public Feedback Feedback { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Feedbacks.Add(Feedback);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
