using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ConsultantQuestion
{
    public class QuestionsFromCustomerModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public QuestionsFromCustomerModel(IQuestionService questionService, IUserService userService)
        {
            _questionService = questionService;
            _userService = userService;
        }

        public List<Question> CustomerQuestions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string? consultantIdStr = HttpContext.Session.GetString("UserId");
            int consultantId = -1;

            if (!string.IsNullOrEmpty(consultantIdStr) && int.TryParse(consultantIdStr, out int parsedUserId))
            {
                consultantId = parsedUserId;
            }
            if (consultantId == -1)
                return RedirectToPage("/Account/Login");


            var questions = await _questionService.GetQuestionsByConsultantIdAsync(consultantId);
            foreach (var q in questions)
            {
                q.User = await _userService.GetUserById(q.UserId);
            }

            CustomerQuestions = questions
                .OrderByDescending(q => q.CreatedAt)
                .ToList();

            return Page();
        }
    }
}
