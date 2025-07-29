using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin;

public class DashboardModel(
    ITestService testService,
    IConsultationService consultationService,
    IUserService userService,
    IFeedbackService feedbackService,
    IServiceService serviceService
) : PageModel
{
    private readonly ITestService _testService = testService;
    private readonly IConsultationService _consultationService = consultationService;
    private readonly IUserService _userService = userService;
    private readonly IFeedbackService _feedbackService = feedbackService;
    private readonly IServiceService _serviceService = serviceService;

    public List<Metric> Metrics { get; set; } = [];
    public List<BookingRecord> RecentBookings { get; set; } = [];
    public List<Feedback> LatestFeedback { get; set; } = [];

    public string HottestService { get; set; } = "N/A";
    public double AverageRating { get; set; }
    public int FeedbackCount { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var role = HttpContext.Session.GetString("Role");
        if (string.IsNullOrEmpty(role) || role != "Admin")
        {
            return RedirectToPage("/Unauthorized");
        }

        var tests = await _testService.GetAllTest();
        var consultations = await _consultationService.GetAllConsultationsAsync();
        var users = await _userService.GetAllUsersAsync();
        var services = await _serviceService.GetAllAsync();

        // Gộp lịch xét nghiệm và tư vấn làm danh sách "booking"
        var allBookings = tests.Select(t => new BookingRecord(
            $"T-{t.TestId}",
            t.Service?.Name ?? "Unknown",
            t.User?.Username ?? "Unknown",
            t.AppointmentTime.ToString("yyyy-MM-dd"),
            t.Status
        )).Concat(consultations.Select(c => new BookingRecord(
            $"C-{c.ConsultationId}",
            "Chuyên gia tư vấn",
            c.User?.Username ?? "Unknown",
            c.AppointmentTime.ToString("yyyy-MM-dd"),
            c.Status
        ))).OrderByDescending(b => b.Date).Take(5).ToList();

        RecentBookings = allBookings;

        // Metrics
        Metrics = new List<Metric>
        {
            new("Tổng lịch xét nghiệm", tests.Count.ToString(), "Tất cả lịch xét nghiệm đã tạo"),
            new("Tổng lịch tư vấn", consultations.Count.ToString(), "Tất cả lịch tư vấn đã tạo"),
            new("Tổng người dùng", users.Count.ToString(), "Người dùng đã đăng ký"),
            new("Tổng dịch vụ", services.Count.ToString(), "Số lượng dịch vụ hiện có")
        };

        // Feedback
        LatestFeedback = await _feedbackService.GetLatestFeedbackAsync(5) ?? [];
        var validRatings = LatestFeedback.Where(f => f.Rating.HasValue && f.Rating.Value >= 0 && f.Rating.Value <= 5).ToList();

        if (validRatings.Any())
        {
            AverageRating = validRatings.Average(f => f.Rating!.Value);
            FeedbackCount = validRatings.Count;
        }

        // Dịch vụ hot nhất
        var serviceFrequency = allBookings
            .GroupBy(b => b.Service)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

        HottestService = serviceFrequency ?? "N/A";

        return Page();
    }

    public record Metric(string Title, string Value, string Description);
    public record BookingRecord(string Id, string Service, string User, string Date, string Status);
}
