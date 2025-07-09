using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class MenstrualCycleDAO
    {
        private readonly GenderHealthcareContext _context;

        public MenstrualCycleDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<MenstrualCycle>> GetAllAsync()
        {
            return await _context.MenstrualCycles
                                 .Include(mc => mc.User)
                                 .Include(mc => mc.Reminders)
                                 .ToListAsync();
        }

        public async Task<MenstrualCycle?> GetByIdAsync(int id)
        {
            return await _context.MenstrualCycles
                                 .Include(mc => mc.User)
                                 .Include(mc => mc.Reminders)
                                 .FirstOrDefaultAsync(mc => mc.CycleId == id);
        }

        public async Task<List<MenstrualCycle>> GetByUserIdAsync(int userId)
        {
            return await _context.MenstrualCycles
                                 .Where(mc => mc.UserId == userId)
                                 .Include(mc => mc.Reminders)
                                 .OrderByDescending(mc => mc.StartDate)
                                 .ToListAsync();
        }

        public async Task<bool> AddAsync(MenstrualCycle cycle)
        {
            try
            {
                await _context.MenstrualCycles.AddAsync(cycle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MenstrualCycleDAO][AddAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(MenstrualCycle cycle)
        {
            try
            {
                _context.MenstrualCycles.Update(cycle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MenstrualCycleDAO][UpdateAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var cycle = await GetByIdAsync(id);
                if (cycle != null)
                {
                    _context.MenstrualCycles.Remove(cycle);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MenstrualCycleDAO][DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }
    }
}
