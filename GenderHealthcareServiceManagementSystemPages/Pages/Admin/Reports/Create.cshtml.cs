using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Text;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.Reports
{
    public class CreateModel(IReportService service, IWebHostEnvironment env) : PageModel
    {
        private readonly IReportService _service = service;
        private readonly IWebHostEnvironment _env = env;
        [BindProperty]
        public string ReportType { get; set; } = string.Empty;

        [BindProperty]
        public DateTime? FromDate { get; set; }

        [BindProperty]
        public DateTime? ToDate { get; set; }

        [BindProperty]
        public string DateRange { get; set; } = "month";

        [BindProperty]
        public string ExportFormat { get; set; } = "pdf";

        [BindProperty]
        public bool AutoEmail { get; set; }

        public void OnGet()
        {
            // Initialize default values
            FromDate = DateTime.Now.AddDays(-30);
            ToDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                byte[] fileBytes;
                string ext;
                string mime;

                if (ExportFormat.ToLower() == "excel")
                {
                    ext = "xlsx";
                    mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileBytes = Encoding.UTF8.GetBytes("Fake Excel Content");
                }
                else
                {
                    ext = "pdf";
                    mime = "application/pdf";
                    fileBytes = Encoding.UTF8.GetBytes("Fake PDF Report Content");
                }

                string folderPath = Path.Combine(_env.WebRootPath, "reports");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);//chưa có thì tạo nha

                string fileName = $"{ReportType}_{DateTime.Now:yyyyMMdd_HHmmss}.{ext}";
                string fullPath = Path.Combine(folderPath, fileName);
                await System.IO.File.WriteAllBytesAsync(fullPath, fileBytes);

                var report = new Report
                {
                    ReportType = ReportType,
                    GeneratedBy = int.Parse(HttpContext.Session.GetString("UserId")),
                    ReportData = $"{fileName}",
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };

                await _service.AddReportAsync(report);

                TempData["SuccessMessage"] = "Báo cáo đã được tạo thành công!";
                return RedirectToPage("/Admin/Reports/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo báo cáo: {ex.Message}";
                return Page();
            }
        }
    }
}
