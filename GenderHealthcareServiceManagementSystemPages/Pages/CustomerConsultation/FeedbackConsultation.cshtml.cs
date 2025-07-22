using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerConsultation
{
    public class FeedbackConsultationModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConsultantInfoService _consultantService;
        private readonly IConsultationService _consultationService;
        private readonly IFeedbackService _feedbackService;

        public FeedbackConsultationModel(
            IUserService userService,
            IConsultantInfoService consultantService,
            IConsultationService consultationService,
            IFeedbackService feedbackService)
        {
            _userService = userService;
            _consultantService = consultantService;
            _consultationService = consultationService;
            _feedbackService = feedbackService;
        }

        public int ConsultationId { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string FeedbackText { get; set; } = "";

        public User? Consultant { get; set; }
        public bool IsReadOnly { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            var consultationId = HttpContext.Session.GetInt32("SelectedConsultationId");
            if (consultationId == null)
                return RedirectToPage("/CustomerConsultation/ShowUserConsultations");

            ConsultationId = consultationId.Value;

            var consultation = await _consultationService.GetConsultationById(ConsultationId);
            if (consultation == null || consultation.Consultant == null)
                return NotFound();

            Consultant = consultation.Consultant;

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdStr, out int userId))
            {
                var existingFeedback = await _feedbackService.GetFeedbackByConsultationIdAsync(userId, ConsultationId);
                if (existingFeedback != null)
                {
                    Rating = existingFeedback.Rating ?? 0;
                    FeedbackText = existingFeedback.FeedbackText ?? "";
                    IsReadOnly = true;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Login");

            var consultationId = HttpContext.Session.GetInt32("SelectedConsultationId");
            if (consultationId == null)
                return RedirectToPage("/CustomerConsultation/ShowUserConsultations");

            ConsultationId = consultationId.Value;

            var consultation = await _consultationService.GetConsultationById(ConsultationId);
            if (consultation == null || consultation.ConsultantId == null)
                return NotFound();

            var existingFeedback = await _feedbackService.GetFeedbackByConsultationIdAsync(userId, ConsultationId);
            if (existingFeedback != null)
            {
                return RedirectToPage("ShowUserConsultations");
            }

            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError("Rating", "Vui lòng chọn số sao từ 1 đến 5.");
                return await OnGetAsync();
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ConsultationId = ConsultationId,
                ConsultantId = consultation.ConsultantId,
                Rating = Rating,
                FeedbackText = FeedbackText,
                CreatedAt = DateTime.Now
            };

            await _feedbackService.AddFeedback(feedback);
            HttpContext.Session.Remove("SelectedConsultationId");

            TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá tư vấn viên!";
            return RedirectToPage("ShowUserConsultations");
        }
    }

}
