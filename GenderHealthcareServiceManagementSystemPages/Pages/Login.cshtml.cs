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
            var userId = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"[LoginModel][OnGetAsync] Kiểm tra session. UserId hiện tại: {userId}");

            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("[LoginModel][OnGetAsync] Người dùng đã đăng nhập, chuyển hướng theo role...");

                var role = HttpContext.Session.GetString("Role");
                return RedirectByRole(role);
            }

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
                HttpContext.Session.SetString("Role", user.Role ?? "Guest");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    Console.WriteLine($"[LoginModel][OnPostAsync] Chuyển hướng đến ReturnUrl: {returnUrl}");
                    return Redirect(returnUrl);
                }

                return RedirectByRole(user.Role);
            }

            Console.WriteLine("[LoginModel][OnPostAsync] Đăng nhập thất bại.");
            Message = "Tên đăng nhập hoặc mật khẩu không đúng.";
            ViewData["ReturnUrl"] = returnUrl;
            return Page();
        }

        private IActionResult RedirectByRole(string? role)
        {
            switch (role)
            {
                case "Admin":
                case "Manager":
                case "Staff":
                    //return RedirectToPage("/Admin/Dashboard");
                    return RedirectToPage("/AdminManageConsultant/ManageConsultant");

                case "Consultant":
                    //return RedirectToPage("/Consultations/Index");
                    return RedirectToPage("/ConsultantQuestion/QuestionsFromCustomer");

                case "Customer":
                case "Guest":
                default:
                    //return RedirectToPage("/Index");
                    return RedirectToPage("/CustomerTesting/SelectService");
            }
        }
    }
}
