using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using Microsoft.Extensions.Hosting;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
public class ReportService(IReportRepository repo, IUserRepository userRepo) : IReportService
{
    private readonly IReportRepository _reportRepository = repo;
    private readonly IUserRepository _userRepository = userRepo;

    public async Task<List<Report>> GetAllReportsAsync()
    {
        return await _reportRepository.GetAllReportsAsync();
    }

    public async Task<Report> GetReportByIdAsync(int reportId)
    {
        return await _reportRepository.GetReportByIdAsync(reportId);
    }

    public async Task AddReportAsync(Report report)
    {
        await _reportRepository.AddReportAsync(report);
    }

    public async Task UpdateReportAsync(Report report)
    {
        await _reportRepository.UpdateReportAsync(report);
    }

    public async Task DeleteReportAsync(string reportId)
    {
        await _reportRepository.DeleteReportAsync(reportId);
    }

    public async Task<List<ReportDisplayModel>> GetAllReportDisplayModelsAsync()
    {
        var reports = await _reportRepository.GetAllReportsAsync();
        var reportDisplayModels = new List<ReportDisplayModel>();
        foreach (var report in reports)
        {
            User? user = null;
            if (report.GeneratedBy != null)
            {
                user = await _userRepository.GetUserById(report.GeneratedBy ?? 0);
            }
            var reportDisplayModel = new ReportDisplayModel
            {
                ReportId = report.ReportId,
                ReportTypeName = ConvertReportTypeToString(report.ReportType),
                CreatedDate = report.CreatedAt ?? DateTime.MinValue,
                CreatedBy = user?.Username,
            };
            reportDisplayModels.Add(reportDisplayModel);
        }
        return reportDisplayModels;
    }
    private string ConvertReportTypeToString(string reportType)
    {
        return reportType switch
        {
            "UserActivity" => "Báo cáo tổng hợp hoạt động người dùng",
            "TestSummary" => "Báo cáo kết quả xét nghiệm",
            "HealthConsultation" => "Báo cáo tư vấn y tế",
            "ServiceUsage" => "Báo cáo sử dụng dịch vụ y tế",
            _ => "Unknown"
        };
    }
}
