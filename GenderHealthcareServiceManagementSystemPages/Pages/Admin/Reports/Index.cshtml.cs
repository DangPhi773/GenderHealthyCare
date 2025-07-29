using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.Reports
{
    [IgnoreAntiforgeryToken]
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
                return NotFound("Báo cáo không tồn tại.");

            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }

            var folderPath = Path.Combine(_env.WebRootPath, "reports");
            var filePath = Path.Combine(folderPath, report.ReportData);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File báo cáo không tồn tại.");
            }

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = GetContentType(filePath);
                return File(fileBytes, contentType, report.ReportData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Có lỗi xảy ra khi đọc file.");
            }
        }

        public async Task<IActionResult> OnPostDeleteExportFileAsync(int id)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                if (string.IsNullOrEmpty(role) || role != "Admin")
                {
                    return RedirectToPage("/Unauthorized");
                }

                var report = await _reportService.GetReportByIdAsync(id);
                if (report == null)
                {
                    return new JsonResult(new { success = false, message = "Báo cáo không tồn tại." })
                    {
                        StatusCode = 404
                    };
                }

                // Xóa file vật lý nếu tồn tại
                var folderPath = Path.Combine(_env.WebRootPath, "reports");
                var filePath = Path.Combine(folderPath, report.ReportData);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Không thể xóa file: {ex.Message}");
                    }
                }

                // Xóa record trong database (soft delete)
                await _reportService.DeleteReportAsync(id);

                TempData["SuccessMessage"] = "Đã xóa báo cáo thành công.";
                return new JsonResult(new { success = true, message = "Đã xóa báo cáo thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa báo cáo: {ex.Message}");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra khi xóa báo cáo." })
                {
                    StatusCode = 500
                };
            }
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
