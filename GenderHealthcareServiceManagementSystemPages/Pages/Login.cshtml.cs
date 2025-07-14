using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [TempData]
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            Console.WriteLine($"[LoginModel][OnGetAsync] Kiểm tra session. UserId hiện tại: {HttpContext.Session.GetString("UserId")}");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                Console.WriteLine("[LoginModel][OnGetAsync] Người dùng đã đăng nhập, chuyển hướng đến /Index");
                return RedirectToPage("/Index");
            }

            // Lưu ReturnUrl vào TempData hoặc ViewData để dùng sau
            ViewData["ReturnUrl"] = returnUrl;
            Console.WriteLine($"[LoginModel][OnGetAsync] ReturnUrl = {returnUrl}");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            Console.WriteLine($"[LoginModel][OnPostAsync] Nhận yêu cầu đăng nhập với Username: {Username}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("[LoginModel][OnPostAsync] ModelState không hợp lệ.");
                Message = "Vui lòng kiểm tra lại thông tin.";
                ViewData["ReturnUrl"] = returnUrl;
                return Page();
            }

            var user = await _accountService.LoginAsync(Username, Password);
            if (user != null)
            {
                Console.WriteLine($"[LoginModel][OnPostAsync] Đăng nhập thành công cho UserId: {user.UserId}");
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Role", user.Role.ToString());

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    Console.WriteLine($"[LoginModel][OnPostAsync] Chuyển hướng đến ReturnUrl: {returnUrl}");
                    return Redirect(returnUrl);
                }
                if (user.Role == "Admin")
                {
                    return RedirectToPage("/Admin/Dashboard");
                }

                return RedirectToPage("/Index");
            }

            Console.WriteLine("[LoginModel][OnPostAsync] Đăng nhập thất bại.");
            Message = "Tên đăng nhập hoặc mật khẩu không đúng.";
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }
    }
}