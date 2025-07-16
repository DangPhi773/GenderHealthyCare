using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting;

public class ScheduleTests : PageModel
{
    private readonly ITestService _testService;

    public ScheduleTests(ITestService testService)
    {
        _testService = testService;
    }

    public List<Test> ScheduledTests { get; set; } = new();

    public async Task OnGetAsync()
    {
        ScheduledTests = await _testService.GetScheduledTests(); 
    }

    [BindProperty]
    public int TestId { get; set; }

    [BindProperty]
    public string Status { get; set; }

    [BindProperty]
    public string Result { get; set; }

    public async Task<IActionResult> OnPostUpdateResultAsync()
    {
        if (string.IsNullOrEmpty(Result))
        {
            TempData["Error"] = "Kết quả không thể để trống!";
            return RedirectToPage();
        }

        var result = await _testService.UpdateTestStatus(TestId, Status, Result);

        if (result)
            TempData["Success"] = "Kết quả đã được lưu!";
        else
            TempData["Error"] = "Cập nhật thất bại.";

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCompleteAsync()
    {
        var test = await _testService.GetTestById(TestId);
        if (test != null && string.IsNullOrEmpty(test.Result))
        {
            TempData["Error"] = "Kết quả xét nghiệm phải được nhập trước khi đánh dấu hoàn tất.";
            return RedirectToPage();
        }
        
        var result = await _testService.UpdateTestStatus(TestId, "Completed");

        if (result)
        {
            TempData["Success"] = "Lịch hẹn đã được hoàn tất!";
        }
        else
        {
            TempData["Error"] = "Cập nhật thất bại.";
        }

        return RedirectToPage();
    }
}