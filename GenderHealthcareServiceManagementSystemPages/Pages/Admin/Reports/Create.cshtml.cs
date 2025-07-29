using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Text;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.Reports
{
    public class CreateModel(IReportService reportService, IReportExportService reportExportService, IWebHostEnvironment env) : PageModel
    {
        private readonly IReportService _reportService = reportService;
        private readonly IReportExportService _reportExportService = reportExportService;
        private readonly IWebHostEnvironment _env = env;
        [BindProperty]
        public string ReportType { get; set; } = string.Empty;

        [BindProperty]
        public string DateRange { get; set; } = "month";

        [BindProperty]
        public DateTime? FromDate { get; set; }

        [BindProperty]
        public DateTime? ToDate { get; set; }

        [BindProperty]
        public string ExportFormat { get; set; } = "pdf";

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
                byte[] fileBytes = await _reportExportService.GenerateReportAsync(
                                ReportType,
                                FromDate, ToDate, ExportFormat.ToLower()
                            );
                string ext;
                string mime;

                if (ExportFormat.ToLower() == "excel")
                {
                    ext = "xlsx";
                    mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                else
                {
                    ext = "pdf";
                    mime = "application/pdf";
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

                await _reportService.AddReportAsync(report);

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
