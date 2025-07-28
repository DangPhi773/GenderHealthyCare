using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects;
public class ReportDAO
{
    private readonly GenderHealthcareContext _context;
    public ReportDAO(GenderHealthcareContext context)
    {
        _context = context;
    }
    public async Task<List<Report>> GetAllAsync()
    {
        return await _context.Reports.ToListAsync();
    }
    public async Task<Report?> GetByIdAsync(int id)
    {
        return await _context.Reports.FindAsync(id);
    }
    public async Task AddAsync(Report report)
    {
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Report report)
    {
        _context.Reports.Update(report);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(string id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report != null)
        {
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
