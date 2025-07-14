using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ExpertConsultation
{
    public class QuestionModel : PageModel
    {
        [BindProperty]
        public QuestionRequest QuestionRequest { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Xử lý logic gửi câu hỏi
            // TODO: Lưu vào database, gửi email thông báo, etc.

            // Simulate processing time
            await Task.Delay(1000);

            TempData["SuccessMessage"] = $"Cảm ơn {QuestionRequest.Name}! Câu hỏi '{QuestionRequest.Subject}' đã được gửi thành công. Tư vấn viên sẽ phản hồi qua email {QuestionRequest.Email} trong vòng 24-48 giờ.";

            // Reset form after successful submission
            QuestionRequest = new();

            return RedirectToPage();
        }
    }

    public class QuestionRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        public string AgeRange { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn danh mục câu hỏi")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề câu hỏi")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nội dung câu hỏi")]
        [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
        public string Content { get; set; } = string.Empty;

        public string Urgency { get; set; } = "medium";

        public bool IsAnonymous { get; set; } = false;

        [Required(ErrorMessage = "Vui lòng đồng ý với điều khoản sử dụng")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Vui lòng đồng ý với điều khoản sử dụng")]
        public bool AcceptTerms { get; set; } = false;
    }
}
