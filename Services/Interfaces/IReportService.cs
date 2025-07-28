using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces;
public interface IReportService
{
    Task<List<Report>> GetAllReportsAsync();
    Task<List<ReportDisplayModel>> GetAllReportDisplayModelsAsync();
    Task<Report> GetReportByIdAsync(int reportId);
    Task AddReportAsync(Report report);
    Task UpdateReportAsync(Report report);
    Task DeleteReportAsync(string reportId);
}
