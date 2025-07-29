using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces;
public interface IReportRepository
{
    Task<List<Report>> GetAllReportsAsync();
    Task<Report> GetReportByIdAsync(int reportId);
    Task AddReportAsync(Report report);
    Task UpdateReportAsync(Report report);
    Task DeleteReportAsync(int reportId);
}
