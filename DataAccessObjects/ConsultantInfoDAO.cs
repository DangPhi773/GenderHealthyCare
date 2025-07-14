using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects;
public class ConsultantInfoDAO (GenderHealthcareContext context)
{
    private readonly GenderHealthcareContext _context = context;
    public async Task<List<ConsultantInfo>> GetAllConsultantInfosAsync()
    {
        var data = await _context.ConsultantInfos.Include(c => c.Consultant).ToListAsync();
        return data;
    }

    public async Task<ConsultantInfo?> GetConsultantInfoByIdAsync(int consultantId)
    {
        var found = await _context.ConsultantInfos.Where(c => c.ConsultantId == consultantId).Include(c => c.Consultant).FirstOrDefaultAsync();
        Console.WriteLine($"ConsultantInfoDAO: GetConsultantInfoByIdAsync({consultantId}) => {found?.Consultant?.FullName ?? "null"}");
        return found;
    }
    public async Task<bool> CreateConsultantInfoAsync(ConsultantInfo info)
    {
        try
        {
            await _context.ConsultantInfos.AddAsync(info);
            await _context.SaveChangesAsync();
            Console.WriteLine($"ConsultantInfoDAO: Đã tạo ConsultantInfo cho ConsultantId = {info.ConsultantId}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ConsultantInfoDAO: ❌ Lỗi khi tạo ConsultantInfo: {ex.Message}");
            return false;
        }
    }
}
