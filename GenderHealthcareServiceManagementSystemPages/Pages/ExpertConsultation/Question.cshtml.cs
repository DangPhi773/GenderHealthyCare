using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BusinessObjects.ViewModels;
using Services.Interfaces;
using BusinessObjects.Models;
using Services;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ExpertConsultation
{
    public class QuestionModel(IQuestionService service, IUserService userService, IConsultantInfoService consultantInfoService) : PageModel
    {
        private readonly IQuestionService _service = service;
        private readonly IUserService _userService = userService;
        private readonly IConsultantInfoService _consultantInfoService = consultantInfoService;

        [BindProperty]
        public QuestionRequest QuestionRequest { get; set; } = new();
        public List<ConsultantInfo> ConsultantInfos { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {

            // Check if user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt câu hỏi.";
                return RedirectToPage("/Login", new { returnUrl = "/ExpertConsultation/Question" });
            }
            ConsultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Xử lý logic gửi câu hỏi
            await _service.AddQuestionAsync(QuestionRequest, int.Parse(HttpContext.Session.GetString("UserId")!));
            
            // Simulate processing time
            await Task.Delay(1000);

            var user = await _userService.GetUserById(int.Parse(HttpContext.Session.GetString("UserId")!));
            TempData["SuccessMessage"] = $"Cảm ơn {user?.Username ?? "#UNKNOWN"}! Câu hỏi '{QuestionRequest.Subject}' đã được gửi thành công. Tư vấn viên sẽ phản hồi trong vòng 24-48 giờ.";


            // Reset form after successful submission
            QuestionRequest = new();

            return RedirectToPage();
        }

        public string GetGenderDisplayText(string? gender)
        {
            return gender?.ToLower() switch
            {
                "female" => "Nữ",
                "male" => "Nam",
                _ => "Không xác định"
            };
        }
    }
}
