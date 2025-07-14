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

    public async Task<bool> UpdateConsultantInfoAsync(ConsultantInfo info)
    {
        try
        {
            _context.ConsultantInfos.Update(info);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsultantInfoDAO][Update] Lỗi: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteConsultantInfoAsync(int consultantId)
    {
        try
        {
            var info = await _context.ConsultantInfos
                .FirstOrDefaultAsync(c => c.ConsultantId == consultantId);

            if (info == null)
                return false;

            _context.ConsultantInfos.Remove(info);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsultantInfoDAO][Delete] Lỗi: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> AddConsultantInfoAsync(ConsultantInfo info)
    {
        try
        {
            await _context.ConsultantInfos.AddAsync(info);
            await _context.SaveChangesAsync();
            Console.WriteLine($"[ConsultantInfoDAO][Add] Đã thêm ConsultantInfo cho UserId: {info.ConsultantId}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ConsultantInfoDAO][Add] Lỗi khi thêm: {ex.Message}");
            return false;
        }
    }

}
