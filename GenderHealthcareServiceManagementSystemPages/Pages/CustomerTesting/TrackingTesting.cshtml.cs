using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using global::Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerTesting
{
    public class TrackingTestingModel : PageModel
    {
        private readonly ITestService _testService;

        public TrackingTestingModel(ITestService testService)
        {
            _testService = testService;
        }

        public List<Test> UserTests { get; set; } = new();
        public string? SelectedStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(string? status)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/CustomerTesting/SelectService");
            }

            var allTests = await _testService.GetTestsByUserId(userId.Value);

            if (!string.IsNullOrEmpty(status))
            {
                SelectedStatus = status;
                UserTests = allTests.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                UserTests = allTests;
            }

            return Page();
        }
    }
}
