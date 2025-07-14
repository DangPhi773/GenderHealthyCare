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
        private readonly IFeedbackService _feedbackService;

        public FeedbackConsultationModel(IUserService userService, IConsultantInfoService consultantService, IFeedbackService feedbackService)
        {
            _userService = userService;
            _consultantService = consultantService;
            _feedbackService = feedbackService;
        }

        [BindProperty(SupportsGet = true)]
        public int ConsultantId { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string FeedbackText { get; set; } = "";

        public User? Consultant { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Consultant = await _userService.GetUserById(ConsultantId);
            if (Consultant == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Login");

            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError("Rating", "Vui lòng chọn số sao từ 1 đến 5.");
                return Page();
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ConsultantId = ConsultantId,
                Rating = Rating,
                FeedbackText = FeedbackText,
                CreatedAt = DateTime.Now
            };

            await _feedbackService.AddFeedback(feedback);
            TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá tư vấn viên!";
            return RedirectToPage("ShowUserConsultations");
        }
    }
}
