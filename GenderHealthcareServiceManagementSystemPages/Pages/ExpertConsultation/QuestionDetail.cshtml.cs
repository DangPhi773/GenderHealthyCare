using DataAccessObjects;
using BusinessObjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ExpertConsultation
{
    public class QuestionDetailModel(IQuestionService service) : PageModel
    {
        private readonly IQuestionService _service = service;

        public QuestionDisplayModel Question { get; set; } = new();

        public async Task<IActionResult> OnGet(int id)
        {
            //// Simulate fetching question details from a database
            var question = await _service.GetQuestionByIdAsync(id);


            if (question == null)
            {
                return NotFound(); // Return 404 if question not found
            }

            Question = new QuestionDisplayModel()
            {
                Id = question.QuestionId,
                UserId = question.UserId,
                Answer = question.AnswerText ?? "",
                Subject = question.QuestionText.Split('-')[0],
                Content = question.QuestionText.Split('-')[1],
                SubmissionDate = question.CreatedAt,
                Status = question.Status ?? "Pending",
            };

            return Page();
        }
    }
}
