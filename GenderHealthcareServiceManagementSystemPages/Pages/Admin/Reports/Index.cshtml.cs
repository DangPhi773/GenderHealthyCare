using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.Reports
{
    public class IndexModel(IReportService reportService, IReportExportService exportService, IWebHostEnvironment env) : PageModel
    {
        private readonly IReportService _reportService = reportService;
        private readonly IReportExportService _exportService = exportService;
        private readonly IWebHostEnvironment _env = env;

        public List<ReportDisplayModel>? Reports { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }
            Reports = await _reportService.GetAllReportDisplayModelsAsync();

            return Page();
        }

        public async Task<IActionResult> OnGetExportFileAsync(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null || report.IsDeleted == true)
                return NotFound();

            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }

            var folderPath = Path.Combine(_env.WebRootPath, "reports");
            var filePath = Path.Combine(folderPath, report.ReportData);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(filePath);
            return File(fileBytes, contentType, report.ReportData);
        }

        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }
    }
}
