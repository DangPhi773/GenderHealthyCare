using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using GenderHealthcareServiceManagementSystemPages.Hubs;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ConsultantQuestion
{
    public class AnswerCustomerQuestionModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;
        private readonly IHubContext<SignalRServer> _hubcontext;

        public AnswerCustomerQuestionModel(IQuestionService questionService, IUserService userService, IHubContext<SignalRServer> hubContext)
        {
            _questionService = questionService;
            _userService = userService;
            _hubcontext = hubContext;
        }

        [BindProperty]
        public string AnswerText { get; set; } = string.Empty;

        public Question? Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Question = await _questionService.GetQuestionByIdAsync(id);
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
                Question = await _questionService.GetQuestionByIdAsync(id);
                Question.User = await _userService.GetUserById(Question.UserId);
                return Page();
            }

            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();

            question.AnswerText = AnswerText.Trim();
            question.AnsweredAt = DateTime.UtcNow.AddHours(7);
            question.Status = "Answered";
            await _questionService.UpdateQuestionAsync(question);

            await _hubcontext.Clients.All.SendAsync("QuestionAnswered", new
            {
                QuestionId = question.QuestionId,
                Answer = question.AnswerText,
                Status = question.Status,
                AnsweredAt = question.AnsweredAt?.ToString("HH:mm dd/MM/yyyy")
            });


            TempData["Message"] = "Đã trả lời câu hỏi thành công.";
            return RedirectToPage("QuestionsFromCustomer");
        }
    }
}
