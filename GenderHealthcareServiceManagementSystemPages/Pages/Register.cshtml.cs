using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public User User { get; set; } = new User();

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please correct the input errors.";
                return Page();
            }

            if (await _accountService.RegisterAsync(User))
            {
                Message = "Registration successful! Please log in.";
                return RedirectToPage("/Login");
            }
            Message = "Username or email already exists.";
            return Page();
        }
    }
}