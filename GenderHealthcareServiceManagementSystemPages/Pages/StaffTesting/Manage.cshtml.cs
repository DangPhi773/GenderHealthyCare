using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting;

public class Manage : PageModel
{
    private readonly ITestService _testService;

    public Manage(ITestService testService)
    {
        _testService = testService;
    }

    public List<Test> PendingTests { get; set; } = new();

    [BindProperty]
    public int TestId { get; set; }

    [BindProperty]
    public string Status { get; set; }
    
    [BindProperty]
    public string Result { get; set; }

    public async Task OnGetAsync()
    {
        PendingTests = await _testService.GetPendingTests();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync()
    {
        var result = await _testService.UpdateTestStatus(TestId, Status, Result);

        if (result)
            TempData["Success"] = $"Đã cập nhật lịch {TestId} thành '{Status}'.";
        else
            TempData["Error"] = $"Cập nhật thất bại cho lịch {TestId}.";

        return RedirectToPage();
    }
}