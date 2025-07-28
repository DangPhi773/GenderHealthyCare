using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<List<Test>> GetAllTest();
        Task<Test?> GetTestById(int id);
        Task AddTest(Test test);
        Task UpdateTest(Test test);
        Task DeleteAsync(int id);
        Task<List<Test>> GetTestsByUserId(int id);
        Task<List<Test>> GetPendingTests();
        Task<List<Test>> GetScheduledTests();
        Task<bool> UpdateTestStatus(int testId, string status);
        Task<bool> UpdateTestResultOrCancel(int testId, string result, string cancelReason);
        Task<bool> IsAppointmentTimeTestingConflict(int userId, DateTime selectedTime);
        Task<bool> UpdateTestFields(int testId, string? status = null, string? result = null, string? cancelReason = null);

    }
}
