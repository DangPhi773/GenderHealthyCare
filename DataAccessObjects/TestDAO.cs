using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TestDAO
    {
        private readonly GenderHealthcareContext _context;

        public TestDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Test>> GetAllTest()
        {
            return await _context.Tests
                .Include(t => t.User)
                .Include(t => t.Service)
                .ToListAsync();
        }

        public async Task<Test?> GetTestById(int id)
        {
            return await _context.Tests
                .Include(t => t.User)
                .Include(t => t.Service)
                .FirstOrDefaultAsync(t => t.TestId == id);
        }

        public async Task AddTest(Test test)
        {
            await _context.Tests.AddAsync(test);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTest(Test test)
        {
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test != null && test.Status == "Pending")
            {
                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Test>> GetTestsByUserId(int id)
        {
            return await _context.Tests
                .Where(t => t.UserId == id)
                .Include(t => t.Service)
                .OrderByDescending(t => t.AppointmentTime)
                .ToListAsync();
        }

        public async Task<List<Test>> GetPendingTests()
        {
            return await _context.Tests
                .Where(t => t.Status == "Pending")
                .Include(t => t.User)
                .Include(t => t.Service)
                .ToListAsync();
        }
        
        public async Task<List<Test>> GetScheduledTests()
        {
            return await _context.Tests
                .Where(t => t.Status == "Scheduled")
                .Include(t => t.User)
                .Include(t => t.Service)
                .ToListAsync();
        }
        
        public async Task<bool> UpdateTestStatus(int testId, string status)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) return false;

            test.Status = status;
            
            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UpdateTestResultOrCancel(int testId, string result, string cancelReason)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null)
                return false;
            
            if (test.Status != "Scheduled")
                return false;
            
            if (!string.IsNullOrEmpty(result))
            {
                test.Result = result;
                test.Status = "Completed";
            }
            else if (!string.IsNullOrEmpty(cancelReason))
            {
                test.CancelReason = cancelReason;
                test.Status = "Cancelled";
            }
            else
            {
                return false;
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        public async Task<bool> IsAppointmentTimeTestingConflict(int userId, DateTime selectedTime)
        {
            var existingTests = await _context.Tests
                .Where(t => t.UserId == userId && t.Status != "Cancelled")
                .ToListAsync();

            foreach (var test in existingTests)
            {
                if (test.AppointmentTime.Date == selectedTime.Date &&
                    Math.Abs((test.AppointmentTime - selectedTime).TotalMinutes) < 120)
                {
                    return true; 
                }
            }

            return false;
        }
        public async Task<bool> UpdateTestFields(int testId, string? status = null, string? result = null, string? cancelReason = null)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) return false;

            if (status != null)
            {
                test.Status = status;
            }

            if (result != null)
            {
                test.Result = result;
            }

            if (cancelReason != null)
            {
                test.CancelReason = cancelReason;
            }

            _context.Tests.Update(test);
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
