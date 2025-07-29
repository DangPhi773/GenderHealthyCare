using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.UserManager
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel( IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user != null)
            {
                
                await _userService.DeleteUserAsync(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
