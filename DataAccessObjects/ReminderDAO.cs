using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ReminderDAO
    {
        private readonly GenderHealthcareContext _context;

        public ReminderDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Reminder>> GetAllAsync()
        {
            return await _context.Reminders
                                 .Include(r => r.User)
                                 .Include(r => r.Cycle)
                                 .ToListAsync();
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            return await _context.Reminders
                                 .Include(r => r.User)
                                 .Include(r => r.Cycle)
                                 .FirstOrDefaultAsync(r => r.ReminderId == id);
        }

        public async Task<List<Reminder>> GetByUserIdAsync(int userId)
        {
            return await _context.Reminders
                                 .Where(r => r.UserId == userId)
                                 .Select(r => new Reminder
                                 {
                                     ReminderId = r.ReminderId,
                                     UserId = r.UserId,
                                     CycleId = r.CycleId,
                                     ReminderType = r.ReminderType,
                                     ReminderTime = r.ReminderTime,
                                     Message = r.Message,
                                     Status = r.Status,
                                     Cycle = r.Cycle
                                 })
                                 .OrderByDescending(r => r.ReminderTime)
                                 .ToListAsync();
        }

        public async Task<bool> AddAsync(Reminder reminder)
        {
            try
            {
                reminder.User = null;
                reminder.Cycle = null;
                await _context.Reminders.AddAsync(reminder);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderDAO][AddAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Reminder reminder)
        {
            try
            {
                var existingReminder = await _context.Reminders
                                                    .FirstOrDefaultAsync(r => r.ReminderId == reminder.ReminderId);

                if (existingReminder != null)
                {
                    existingReminder.UserId = reminder.UserId;
                    existingReminder.CycleId = reminder.CycleId;
                    existingReminder.ReminderType = reminder.ReminderType;
                    existingReminder.ReminderTime = reminder.ReminderTime;
                    existingReminder.Message = reminder.Message;
                    existingReminder.Status = reminder.Status;
                    var rowsAffected = await _context.SaveChangesAsync();
                    return rowsAffected > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderDAO][UpdateAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var reminder = await GetByIdAsync(id);
                if (reminder != null)
                {
                    _context.Reminders.Remove(reminder);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReminderDAO][DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }
    }
}