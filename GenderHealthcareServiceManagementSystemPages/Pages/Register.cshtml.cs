using BusinessObjects.Models;
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

        [BindProperty]
        public string? Message { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"❌ Model Error: {key} - {error.ErrorMessage}");
                    }
                }

                Message = "Vui lòng kiểm tra lại thông tin.";
                return Page();
            }

            User.Role = "Customer";
            User.CreatedAt = DateTime.Now;
            User.UpdatedAt = DateTime.Now;

            if (await _accountService.RegisterAsync(User))
            {
                return RedirectToPage("/Login");
            }

            Message = "Tên đăng nhập hoặc email đã tồn tại.";
            return Page();
        }
    }
}