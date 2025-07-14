using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        public List<Metric> Metrics { get; set; }
        public List<Booking> RecentBookings { get; set; }
        public List<Feedback> LatestFeedback { get; set; }

        public void OnGet()
        {
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

            LatestFeedback = new List<Feedback>
        {
            new("FB001", "Alice Johnson", 5, "Excellent service, very professional!", "2024-07-10"),
            new("FB002", "Bob Williams", 4, "Consultant was helpful, but wait time was long.", "2024-07-09"),
            new("FB003", "Charlie Brown", 5, "App is intuitive and easy to use for tracking.", "2024-07-08"),
            new("FB004", "Diana Prince", 2, "Booking was cancelled without clear reason.", "2024-07-07"),
        };
        }

        public record Metric(string Title, string Value, string Description);
        public record Booking(string Id, string Service, string User, string Date, string Status);
        public record Feedback(string Id, string User, int Rating, string Comment, string Date);
    }

}
