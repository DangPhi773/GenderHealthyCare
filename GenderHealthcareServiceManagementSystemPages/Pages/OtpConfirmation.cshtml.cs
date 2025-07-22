using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class OtpConfirmationModel(IAccountService accountService) : PageModel
    {
        private readonly IAccountService _accountService = accountService;

        [BindProperty]
        [Required(ErrorMessage = "Mã OTP không được để trống.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã OTP phải có 6 chữ số.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mã OTP chỉ được chứa số.")]
        public string OtpCode { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // Optional: You might want to retrieve the email associated with the OTP session here
            // For example, from TempData or a session variable.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var otpStored = HttpContext.Session.GetString("RegisterOtp");
            var email = HttpContext.Session.GetString("RegisterEmail");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (otpStored == OtpCode)
            {
                var user = await _accountService.GetUserByEmailAsync(email);
                if (user != null)
                {
                    await _accountService.ConfirmEmailAsync(user);

                    HttpContext.Session.Remove("RegisterOtp");
                    HttpContext.Session.Remove("RegisterEmail");

                    TempData["SuccessMessage"] = "Xác thực OTP thành công";
                    return RedirectToPage("Login");
                }
            }
            else
            {
                Message = "Mã OTP không hợp lệ. Vui lòng thử lại.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResendOtpAsync()
        {
            // Simulate resending OTP logic
            // In a real application, you would trigger an email/SMS service to send a new OTP.
            Message = "Mã OTP đã được gửi lại. Vui lòng kiểm tra email của bạn.";
            // Clear the current OTP input for a fresh entry
            OtpCode = string.Empty;
            return Page();
        }
    }
}
