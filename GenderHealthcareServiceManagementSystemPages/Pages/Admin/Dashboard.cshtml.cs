using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

    public int TotalTestCount { get; set; }
    public record Metric(string Title, string Value, string Description);
    public record BookingRecord(string Id, string Service, string User, string Date, string Status);
    public List<TestTypeCount> TestCountByType { get; set; } = new();
    [BindProperty(SupportsGet = true)]
    public string? SortType { get; set; }
    [BindProperty(SupportsGet = true)]
    public DateTime? SelectedDate { get; set; }
    public int FeedbackCount { get; set; }
    public List<TestStatisticByConsultant> StatisticByConsultant { get; set; } = new();
    public List<Feedback> LatestFeedback { get; set; } = new();
    public string HottestService { get; set; } = "N/A";
    public double AverageRating { get; set; }
    public List<BookingRecord> RecentBookings { get; set; } = new();
    public List<TestWithConsultant> TestsWithConsultants { get; set; } = new();
    public List<Metric> Metrics { get; set; } = new();

    public async Task<IActionResult> OnGet()
    {
        var role = HttpContext.Session.GetString("Role");
        if (string.IsNullOrEmpty(role) || role != "Admin")
        {
            return RedirectToPage("/Unauthorized");
        }

        // Lấy tất cả dữ liệu
        var tests = (await _testService.GetAllTest())
            .ToList();
        var consultations = await _consultationService.GetAllConsultationsAsync();
        var users = await _userService.GetAllUsersAsync();
        var services = await _serviceService.GetAllAsync();

        // Áp dụng bộ lọc nếu có
        if (!string.IsNullOrEmpty(SortType) && SelectedDate.HasValue)
        {
            var date = SelectedDate.Value;
            if (SortType == "day")
            {
                tests = tests.Where(t => t.AppointmentTime.Date == date.Date).ToList();
                consultations = consultations.Where(c => c.AppointmentTime.Date == date.Date).ToList();
            }
            else if (SortType == "month")
            {
                tests = tests.Where(t => t.AppointmentTime.Month == date.Month && t.AppointmentTime.Year == date.Year).ToList();
                consultations = consultations.Where(c => c.AppointmentTime.Month == date.Month && c.AppointmentTime.Year == date.Year).ToList();
            }
            else if (SortType == "year")
            {
                tests = tests.Where(t => t.AppointmentTime.Year == date.Year).ToList();
                consultations = consultations.Where(c => c.AppointmentTime.Year == date.Year).ToList();
            }
        }

        // Lấy thông tin các xét nghiệm kèm tên consultant
        TestsWithConsultants = tests
            .Select(t => new TestWithConsultant
            {
                TestId = t.TestId,
                TestName = t.Service?.Name ?? "Không xác định",
                PatientName = t.User?.FullName ?? t.User?.Username ?? "Khách hàng ẩn danh",
                AppointmentTime = t.AppointmentTime,
                Status = t.Status ?? "Đang xử lý"
            })
            .OrderByDescending(t => t.AppointmentTime)
            .ToList();

        // Tạo danh sách booking đã lọc
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
        ))).OrderByDescending(b => b.Date).ToList();

        // Gán dữ liệu đã lọc
        RecentBookings = allBookings.Take(5).ToList();
        TotalTestCount = tests.Count;

        // Thống kê tổng quan
        Metrics = new List<Metric>
        {
            new("Tổng lịch xét nghiệm", tests.Count.ToString(), "Lịch xét nghiệm" + (SortType != null ? $" ({GetFilterDescription()})" : "")),
            new("Tổng lịch tư vấn", consultations.Count.ToString(), "Lịch tư vấn" + (SortType != null ? $" ({GetFilterDescription()})" : "")),
            new("Tổng người dùng", users.Count.ToString(), "Người dùng đã đăng ký"),
            new("Tổng dịch vụ", services.Count.ToString(), "Số lượng dịch vụ hiện có")
        };

        // Thống kê theo loại xét nghiệm
        TestCountByType = tests
            .GroupBy(t => t.Service?.Name ?? "Không xác định")
            .Select(g => new TestTypeCount { ServiceName = g.Key, Count = g.Count() })
            .ToList();

        // Thống kê feedback
        LatestFeedback = await _feedbackService.GetLatestFeedbackAsync(5) ?? new();
        var validRatings = LatestFeedback
            .Where(f => f.Rating.HasValue && f.Rating.Value >= 0 && f.Rating.Value <= 5)
            .ToList();

        if (validRatings.Any())
        {
            AverageRating = validRatings.Average(f => f.Rating!.Value);
            FeedbackCount = validRatings.Count;
        }
        else
        {
            AverageRating = 0;
            FeedbackCount = 0;
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

    private string GetFilterDescription()
    {
        if (string.IsNullOrEmpty(SortType) || !SelectedDate.HasValue)
            return "";

        var date = SelectedDate.Value;
        return SortType switch
        {
            "day" => $"Ngày {date:dd/MM/yyyy}",
            "month" => $"Tháng {date:MM/yyyy}",
            "year" => $"Năm {date:yyyy}",
            _ => ""
        };
    }
}
