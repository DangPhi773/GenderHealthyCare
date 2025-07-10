using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class FeedbackTestingModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IServiceService _serviceService;

        public FeedbackTestingModel(IFeedbackService feedbackService, IServiceService serviceService)
        {
            _feedbackService = feedbackService;
            _serviceService = serviceService;
        }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string FeedbackText { get; set; } = "";

        public Service? CurrentService { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Login");

            CurrentService = await _serviceService.GetByIdAsync(ServiceId);
            if (CurrentService == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Login");

            if (!int.TryParse(userIdStr, out int userId)) return RedirectToPage("/Login");

            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError("Rating", "Vui lòng chọn điểm từ 1 đến 5.");
                return Page();
            }

            var feedback = new Feedback
            {
                UserId = userId,
                ServiceId = ServiceId,
                Rating = Rating,
                FeedbackText = FeedbackText,
                CreatedAt = DateTime.Now
            };

            await _feedbackService.AddFeedback(feedback);

            TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToPage("TrackingTesting");
        }
    }
}

