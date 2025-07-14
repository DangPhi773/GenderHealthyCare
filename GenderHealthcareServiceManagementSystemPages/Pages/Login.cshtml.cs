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

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine($"[LoginModel][OnGetAsync] Kiểm tra session. UserId hiện tại: {HttpContext.Session.GetString("UserId")}");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                Console.WriteLine("[LoginModel][OnGetAsync] Người dùng đã đăng nhập, chuyển hướng đến /Index");
                return RedirectToPage("/Index");
            }
            Console.WriteLine("[LoginModel][OnGetAsync] Không có session, hiển thị trang đăng nhập");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"[LoginModel][OnPostAsync] Nhận yêu cầu đăng nhập với Username: {Username}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("[LoginModel][OnPostAsync] ModelState không hợp lệ:");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"[LoginModel][OnPostAsync] Lỗi ModelState: {key} - {error.ErrorMessage}");
                    }
                }
                Message = "Vui lòng kiểm tra lại thông tin.";
                return Page();
            }

            Console.WriteLine("[LoginModel][OnPostAsync] Gọi AccountService.LoginAsync...");
            var user = await _accountService.LoginAsync(Username, Password);
            if (user != null)
            {
                Console.WriteLine($"[LoginModel][OnPostAsync] Đăng nhập thành công cho UserId: {user.UserId}, Username: {user.Username}");
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                Console.WriteLine("[LoginModel][OnPostAsync] Đã lưu UserId vào session, chuyển hướng đến /Index");
                return RedirectToPage("/Index");
            }

            Console.WriteLine("[LoginModel][OnPostAsync] Đăng nhập thất bại: Tên đăng nhập hoặc mật khẩu không đúng.");
            Message = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return Page();
        }
    }
}