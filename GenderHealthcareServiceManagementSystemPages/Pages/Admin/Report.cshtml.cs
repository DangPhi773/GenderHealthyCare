using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin
{
    public class ReportModel : PageModel
    {
        public List<Report> Reports { get; set; }

        public void OnGet()
        {
            Reports = new List<Report>
        {
            new Report { Id = "RPT001", Name = "Monthly User Growth", Date = "2024-07-01", Type = "User Analytics", Status = "Generated", FileUrl = "#" },
            new Report { Id = "RPT002", Name = "Service Popularity", Date = "2024-06-25", Type = "Service Usage", Status = "Generated", FileUrl = "#" },
            new Report { Id = "RPT003", Name = "Consultant Performance Q2", Date = "2024-06-30", Type = "Consultant Metrics", Status = "Generated", FileUrl = "#" },
            new Report { Id = "RPT004", Name = "Feedback Analysis", Date = "2024-07-05", Type = "Feedback", Status = "Pending" }
        };
        }

        public class Report
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Date { get; set; }
            public string Status { get; set; }
            public string? FileUrl { get; set; }
        }
    }
}
