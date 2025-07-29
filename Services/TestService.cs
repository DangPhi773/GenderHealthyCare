using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _repo;

        public TestService(ITestRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Test>> GetAllTest(DateTime? from, DateTime? to) => _repo.GetAllTest(from, to);
        public Task<Test?> GetTestById(int id) => _repo.GetTestById(id);
        public Task AddTest(Test test) => _repo.AddTest(test);
        public Task UpdateTest(Test test) => _repo.UpdateTest(test);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<List<Test>> GetTestsByUserId(int id) => _repo.GetTestsByUserId(id);
        public Task<List<Test>> GetPendingTests() => _repo.GetPendingTests();

        public Task<List<Test>> GetScheduledTests() => _repo.GetScheduledTests();
        public async Task<bool> UpdateTestStatus(int testId, string status)
        {
            if ((status == "ResultAvailable" || status == "Completed"))
            {
                throw new InvalidOperationException("Kết quả phải được nhập khi trạng thái là Pending.");
            }

            return await _repo.UpdateTestStatus(testId, status);
        }

        public async Task<bool> UpdateTestResultOrCancel(int testId, string result, string cancelReason)
        {

            return await _repo.UpdateTestResultOrCancel(testId, result, cancelReason);
        }


        public Task<bool> IsAppointmentTimeTestingConflict(int userId, DateTime selectedTime) => _repo.IsAppointmentTimeTestingConflict(userId, selectedTime);

        public async Task<bool> UpdateTestFields(int testId, string? status = null, string? result = null, string? cancelReason = null)
        {
            return await _repo.UpdateTestFields(testId, status, result, cancelReason);
        }
    }
}
