using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                .Select(mc => new MenstrualCycle
                                {
                                    CycleId = mc.CycleId,
                                    UserId = mc.UserId,
                                    StartDate = mc.StartDate,
                                    EndDate = mc.EndDate,
                                    OvulationDate = mc.OvulationDate,
                                    PillReminderTime = mc.PillReminderTime,
                                    Notes = mc.Notes,
                                    Reminders = mc.Reminders
                                })
                                .OrderByDescending(mc => mc.StartDate)
                                .ToListAsync();
        }

        public async Task<bool> AddAsync(MenstrualCycle cycle)
        {
            try
            {
                cycle.User = null;
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
                Console.WriteLine($"DAO UpdateAsync: CycleId={cycle.CycleId}, StartDate={cycle.StartDate}, EndDate={cycle.EndDate}, OvulationDate={cycle.OvulationDate}, PillReminderTime={cycle.PillReminderTime}, Notes={cycle.Notes}");
                var existingCycle = await _context.MenstrualCycles
                    .Include(c => c.User)
                    .Include(c => c.Reminders)
                    .FirstOrDefaultAsync(c => c.CycleId == cycle.CycleId);
                if (existingCycle != null)
                {
                    existingCycle.StartDate = cycle.StartDate;
                    existingCycle.EndDate = cycle.EndDate;
                    existingCycle.OvulationDate = cycle.OvulationDate;
                    existingCycle.PillReminderTime = cycle.PillReminderTime;
                    existingCycle.Notes = cycle.Notes;
                    var rowsAffected = await _context.SaveChangesAsync();
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                    return rowsAffected > 0;
                }
                return false;
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

        // New method: Calculate average cycle length for a user
        public async Task<double> GetAverageCycleLengthAsync(int userId)
        {
            var cycles = await _context.MenstrualCycles
                                      .Where(mc => mc.UserId == userId && mc.StartDate != null && mc.EndDate != null)
                                      .ToListAsync();

            var validCycles = cycles
                .Where(c => c.CycleLength >= 20 && c.CycleLength <= 35) // Loại bỏ chu kỳ bất thường
                .ToList();

            if (!validCycles.Any())
                return 28; // Giá trị mặc định nếu không có chu kỳ hợp lệ

            return validCycles.Average(c => c.CycleLength);
        }

        // New method: Predict next cycle start date
        public async Task<DateOnly?> PredictNextCycleStartAsync(int userId)
        {
            var cycles = await _context.MenstrualCycles
                                      .Where(mc => mc.UserId == userId && mc.StartDate != null)
                                      .OrderByDescending(mc => mc.StartDate)
                                      .ToListAsync();

            if (!cycles.Any())
                return null;

            var averageCycleLength = await GetAverageCycleLengthAsync(userId);
            var lastCycle = cycles.FirstOrDefault();
            return lastCycle?.StartDate?.AddDays((int)averageCycleLength);
        }

        public async Task<DateOnly?> PredictOvulationDateAsync(int userId, DateOnly cycleStartDate)
        {
            var averageCycleLength = await GetAverageCycleLengthAsync(userId);
            return cycleStartDate.AddDays((int)(averageCycleLength / 2));
        }
        public async Task<DateOnly?> PredictCycleEndDateAsync(int userId, DateOnly cycleStartDate)
        {
            var averageCycleLength = await GetAverageCycleLengthAsync(userId);
            return cycleStartDate.AddDays((int)averageCycleLength - 1);
        }
        public async Task<string> AnalyzeCurrentCycleAsync(int userId)
        {
            var cycles = await _context.MenstrualCycles
                                      .Where(mc => mc.UserId == userId && mc.StartDate != null)
                                      .OrderByDescending(mc => mc.StartDate)
                                      .ToListAsync();

            if (!cycles.Any())
                return "Không đủ dữ liệu";

            var currentCycle = cycles.FirstOrDefault(c => c.EndDate == null || c.EndDate.Value.ToDateTime(TimeOnly.MinValue) >= DateTime.Today);
            var averageCycleLength = await GetAverageCycleLengthAsync(userId);
            var predictedNextStart = await PredictNextCycleStartAsync(userId);

            if (predictedNextStart != null && DateTime.Today >= predictedNextStart.Value.ToDateTime(TimeOnly.MinValue))
            {
                if (currentCycle == null || (currentCycle.EndDate != null && currentCycle.EndDate.Value.ToDateTime(TimeOnly.MinValue) < DateTime.Today))
                    return "Trễ";
            }

            if (currentCycle == null || currentCycle.StartDate == null)
                return "Không có chu kỳ đang diễn ra";

            var daysSinceStart = (DateTime.Today - currentCycle.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days;

            const int normalThreshold = 5; 
            const int abnormalThreshold = 35; 

            if (currentCycle.EndDate == null && daysSinceStart > abnormalThreshold)
                return "Kéo dài bất thường";

            if (currentCycle.EndDate != null)
            {
                var cycleLength = (currentCycle.EndDate.Value.ToDateTime(TimeOnly.MinValue) - currentCycle.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days + 1;
                if (cycleLength >= 20 && cycleLength <= 35 && Math.Abs(cycleLength - averageCycleLength) <= normalThreshold)
                    return "Bình thường";
                if (cycleLength < averageCycleLength - normalThreshold)
                    return "Sớm";
            }

            var previousCycle = cycles.Skip(1).FirstOrDefault();
            if (previousCycle != null && previousCycle.StartDate != null)
            {
                var expectedNextStart = previousCycle.StartDate.Value.AddDays((int)averageCycleLength);
                if (currentCycle.StartDate.Value.ToDateTime(TimeOnly.MinValue) < expectedNextStart.ToDateTime(TimeOnly.MinValue))
                    return "Sớm";
            }

            return "Bình thường";
        }

    }
}