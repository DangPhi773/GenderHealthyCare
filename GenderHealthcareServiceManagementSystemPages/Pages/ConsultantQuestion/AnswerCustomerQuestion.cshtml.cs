using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ConsultantQuestion
{
    public class AnswerCustomerQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public AnswerCustomerQuestionModel(IQuestionService questionService, IUserService userService)
        {
            _questionService = questionService;
            _userService = userService;
        }

        [BindProperty]
        public string AnswerText { get; set; } = string.Empty;

        public Question? Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Question = await _questionService.GetQuestionById(id);
            if (Question == null) return NotFound();

            Question.User = await _userService.GetUserById(Question.UserId);
            AnswerText = Question.AnswerText ?? string.Empty;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (string.IsNullOrWhiteSpace(AnswerText))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập nội dung trả lời.");
                Question = await _questionService.GetQuestionById(id);
                Question.User = await _userService.GetUserById(Question.UserId);
                return Page();
            }

            var question = await _questionService.GetQuestionById(id);
            if (question == null) return NotFound();

            question.AnswerText = AnswerText.Trim();
            question.Status = "Answered";
            await _questionService.UpdateQuestion(question);

            TempData["Message"] = "Đã trả lời câu hỏi thành công.";
            return RedirectToPage("QuestionsFromCustomer");
        }
    }
}
