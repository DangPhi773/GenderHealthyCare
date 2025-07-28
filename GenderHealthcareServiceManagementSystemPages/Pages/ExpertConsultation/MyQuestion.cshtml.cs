using DataAccessObjects;
using BusinessObjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ExpertConsultation
{
    public class MyQuestionModel(IQuestionService service) : PageModel
    {
        private readonly IQuestionService _service = service;

        public List<QuestionDisplayModel> UserQuestions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int currentUserId))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem câu hỏi đã đặt.";
                return RedirectToPage("/Login", new { returnUrl = "/ExpertConsultation/MyQuestion" });
            }

            // Simulate fetching questions for the current user
            // In a real application, you would query your database here
            UserQuestions = (await _service.GetQuestionsByUserIdAsync(currentUserId)).Select(q =>
             new QuestionDisplayModel()
             {
                 Id = q.QuestionId,
                 UserId = q.UserId,
                 Subject = q.QuestionText.Split('-')[0],
                 Content = q.QuestionText.Split('-')[1],
                 Status = q.Status,
                 SubmissionDate = q.CreatedAt,
             }
            ).ToList();

            return Page();
        }

    }
}
