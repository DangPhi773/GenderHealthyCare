using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories;
public class ReportRepository(ReportDAO dao) : IReportRepository
{
    private readonly ReportDAO _dao = dao;
    public async Task AddReportAsync(Report report)
    {
        await _dao.AddAsync(report);
    }

    public async Task DeleteReportAsync(int reportId)
    {
        await _dao.DeleteAsync(reportId);
    }

    public async Task<List<Report>> GetAllReportsAsync()
    {
        return await _dao.GetAllAsync();
    }

    public async Task<Report> GetReportByIdAsync(int reportId)
    {
        return await _dao.GetByIdAsync(reportId);
    }

    public async Task UpdateReportAsync(Report report)
    {
        await _dao.UpdateAsync(report);
    }
}
