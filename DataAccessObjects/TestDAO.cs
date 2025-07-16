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
                .ToListAsync();
        }
        
        public async Task<bool> UpdateTestStatus(int testId, string status, string result = null)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) return false;

            test.Status = status;

            if (status == "ResultAvailable" && !string.IsNullOrEmpty(result))
            {
                test.Result = result;
            }

            if (status == "Completed")
            {
                test.Result = string.IsNullOrEmpty(test.Result) ? "No result provided" : test.Result;
            }

            _context.Tests.Update(test);
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}
