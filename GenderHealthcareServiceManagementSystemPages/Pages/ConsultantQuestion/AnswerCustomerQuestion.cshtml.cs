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
        private readonly IEmailService _emailService;

        public AnswerCustomerQuestionModel(IQuestionService questionService, IUserService userService, IHubContext<SignalRServer> hubContext, IEmailService emailService)
        {
            _questionService = questionService;
            _userService = userService;
            _hubcontext = hubContext;
            _emailService = emailService;
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

            // Cập nhật câu hỏi
            question.AnswerText = AnswerText.Trim();
            question.AnsweredAt = DateTime.UtcNow.AddHours(7); // Giờ VN
            question.Status = "Answered";
            await _questionService.UpdateQuestionAsync(question);

            // Gửi thông báo real-time bằng SignalR
            await _hubcontext.Clients.All.SendAsync("QuestionAnswered", new
            {
                QuestionId = question.QuestionId,
                Answer = question.AnswerText,
                Status = question.Status,
                AnsweredAt = question.AnsweredAt?.ToString("HH:mm dd/MM/yyyy")
            });

            // Gửi email phản hồi cho người dùng
            var user = await _userService.GetUserById(question.UserId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                try
                {
                    string subject = "Phản hồi câu hỏi từ tư vấn viên";
                    string message = $@"
                <div style='font-family:Arial,sans-serif;'>
                    <h3 style='color:#2E86C1;'>Tư vấn viên đã phản hồi câu hỏi của bạn</h3>
                    <p>Chào {user.FullName ?? "bạn"},</p>

                    <p><strong>Câu hỏi của bạn:</strong></p>
                    <blockquote style='border-left:4px solid #ccc; padding-left:10px; color:#555;'>
                        {question.QuestionText}
                    </blockquote>

                    <p><strong>Câu trả lời từ tư vấn viên:</strong></p>
                    <blockquote style='border-left:4px solid #2E86C1; padding-left:10px; color:#000;'>
                        {question.AnswerText}
                    </blockquote>

                    <p>Thời gian trả lời: <strong>{question.AnsweredAt?.ToString("HH:mm dd/MM/yyyy")}</strong></p>

                    <p style='margin-top:20px;'>Trân trọng,<br/>Trung tâm y tế</p>
                    <hr/>
                    <small style='color:gray;'>Gender Healthcare System.</small>
                </div>";

                    await _emailService.SendEmailAsync(user.Email, subject, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                }
            }

            TempData["Message"] = "Đã trả lời câu hỏi thành công.";
            return RedirectToPage("QuestionsFromCustomer");
        }
    }
    }
