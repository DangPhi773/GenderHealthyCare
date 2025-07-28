using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin
{
    public class DashboardModel(IFeedbackService feedbackService) : PageModel
    {
        private readonly IFeedbackService _feedbackService = feedbackService;
        public List<Metric> Metrics { get; set; }
        public List<Booking> RecentBookings { get; set; }
        public List<Feedback> LatestFeedback { get; set; } = [];

        public string HottestService { get; set; } // New property for hottest service
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }

        // properties for filtering
        //public int SelectedReviewCount { get; set; } = 5; // Default to 5 reviews
        //public DateTime? StartDate { get; set; } // New property for start date
        //public DateTime? EndDate { get; set; }   // New property for end date

        public async Task<IActionResult> OnGet(int? reviewCount, DateTime? startDate, DateTime? endDate)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }
            Metrics = new List<Metric>
            {
                new("Total Requests", "1,234", "+20.1% from last month"),
                new("New Users", "573", "+180.1% from last month"),
                new("Service Bookings", "890", "+19% from last month"),
                new("Average Rating", "4.7", "Based on 1,200 reviews")
            };

            RecentBookings = new List<Booking>
            {
                new("BK001", "STI Screening", "Alice Johnson", "2024-07-10", "Completed"),
                new("BK002", "Fertility Consultation", "Bob Williams", "2024-07-09", "Pending"),
                new("BK003", "Menstrual Cycle Tracking Setup", "Charlie Brown", "2024-07-08", "Completed"),
                new("BK004", "STI Screening", "Diana Prince", "2024-07-07", "Cancelled"),
                new("BK005", "Online Consultation", "Eve Adams", "2024-07-06", "Completed"),
            };

            //StartDate = startDate;
            //EndDate = endDate;
            LatestFeedback = await _feedbackService.GetLatestFeedbackAsync(reviewCount ?? 5) ?? [];

            if (LatestFeedback != null && LatestFeedback.Any())
            {
                var validRatings = LatestFeedback.Where(f => f.Rating.HasValue && f.Rating.Value >= 0 && f.Rating.Value <= 5).ToList();
                if (validRatings.Any())
                {
                    AverageRating = validRatings.Average(f => f.Rating.Value);
                    FeedbackCount = validRatings.Count();
                }
                else
                {
                    AverageRating = 0;
                    FeedbackCount = 0;
                }
            }
            else
            {
                AverageRating = 0;
                FeedbackCount = 0;
            }
            if (RecentBookings != null && RecentBookings.Any())
            {
                HottestService = RecentBookings
                    .GroupBy(b => b.Service)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault();
            }
            else
            {
                HottestService = "N/A";
            }
            return Page();
        }

        public record Metric(string Title, string Value, string Description);
        public record Booking(string Id, string Service, string User, string Date, string Status);
    }

}
