using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class TrackingTestingModel : PageModel
    {
        private readonly ITestService _testService;
        private readonly IFeedbackService _feedbackService;

        public TrackingTestingModel(ITestService testService, IFeedbackService feedbackService)
        {
            _testService = testService;
            _feedbackService = feedbackService;
        }

        public List<Test> UserTests { get; set; } = new();
        public string? SelectedStatus { get; set; }
        public Dictionary<int, bool> FeedbackStatus { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string? status)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Login");

            var allTests = await _testService.GetTestsByUserId(userId);

            if (!string.IsNullOrEmpty(status))
            {
                SelectedStatus = status;
                UserTests = allTests.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                UserTests = allTests;
            }

            foreach (var test in UserTests)
            {
                var feedback = await _feedbackService.GetFeedbackByTestIdAsync(userId, test.TestId);
                FeedbackStatus[test.TestId] = feedback != null;
            }

            return Page();
        }

        public IActionResult OnPostSetTestSession(int testId)
        {
            HttpContext.Session.SetInt32("SelectedTestId", testId);
            return RedirectToPage("/CustomerTesting/FeedbackTesting");
        }
    }

}
