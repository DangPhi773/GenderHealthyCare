using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.UserManager
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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

            var user = await _userService.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
      


            var existingUser = await _userService.GetUserById(User.UserId);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.FullName = User.FullName;
            existingUser.Email = User.Email;
            existingUser.Role = User.Role;
            existingUser.IsDeleted = User.IsDeleted;
            existingUser.Gender = User.Gender;
            existingUser.Dob = User.Dob;
            existingUser.Phone = User.Phone;
            existingUser.UpdatedAt = DateTime.UtcNow;

            var result = await _userService.UpdateUser(existingUser);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update user.");
                return Page();
            }
            return RedirectToPage("./Index");
        }


    }
}





