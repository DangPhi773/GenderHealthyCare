using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class FeedbackTestingModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ITestService _testService;

        public FeedbackTestingModel(IFeedbackService feedbackService, ITestService testService)
        {
            _feedbackService = feedbackService;
            _testService = testService;
        }

        public int TestId { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string FeedbackText { get; set; } = "";

        public Service? CurrentService { get; set; }
        public Test? CurrentTest { get; set; }
        public bool IsReadOnly { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Login");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToPage("/Login");

            var testId = HttpContext.Session.GetInt32("SelectedTestId");
            if (testId == null) return RedirectToPage("/CustomerTesting/TrackingTesting");

            TestId = testId.Value;
            CurrentTest = await _testService.GetTestById(TestId);
            if (CurrentTest == null || CurrentTest.Service == null) return NotFound();

            CurrentService = CurrentTest.Service;

            var existingFeedback = await _feedbackService.GetFeedbackByTestIdAsync(userId, TestId);
            if (existingFeedback != null)
            {
                Rating = existingFeedback.Rating ?? 0;
                FeedbackText = existingFeedback.FeedbackText ?? "";
                IsReadOnly = true;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Login");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToPage("/Login");

            var testId = HttpContext.Session.GetInt32("SelectedTestId");
            if (testId == null) return RedirectToPage("/CustomerTesting/TrackingTesting");

            TestId = testId.Value;

            var existingFeedback = await _feedbackService.GetFeedbackByTestIdAsync(userId, TestId);
            if (existingFeedback != null)
            {
                return RedirectToPage("TrackingTesting");
            }

            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError("Rating", "Vui lòng chọn điểm từ 1 đến 5.");
                return await OnGetAsync();
            }

            var test = await _testService.GetTestById(TestId);
            if (test == null) return NotFound();

            var feedback = new Feedback
            {
                UserId = userId,
                TestId = TestId,
                ServiceId = test.ServiceId,
                Rating = Rating,
                FeedbackText = FeedbackText,
                CreatedAt = DateTime.Now
            };

            await _feedbackService.AddFeedback(feedback);
            HttpContext.Session.Remove("SelectedTestId");

            TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá!";
            return RedirectToPage("TrackingTesting");
        }
    }
}
