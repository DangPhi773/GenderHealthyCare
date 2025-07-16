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

        public Task<List<Test>> GetAllTest() => _repo.GetAllTest();
        public Task<Test?> GetTestById(int id) => _repo.GetTestById(id);
        public Task AddTest(Test test) => _repo.AddTest(test);
        public Task UpdateTest(Test test) => _repo.UpdateTest(test);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<List<Test>> GetTestsByUserId(int id) => _repo.GetTestsByUserId(id);
        public Task<List<Test>> GetPendingTests() => _repo.GetPendingTests();
        
        public Task<List<Test>> GetScheduledTests() => _repo.GetScheduledTests();
        public async Task<bool> UpdateTestStatus(int testId, string status, string result = null)
        {
            if (status == "ResultAvailable" && string.IsNullOrEmpty(result))
            {
                throw new InvalidOperationException("Kết quả phải được nhập khi trạng thái là 'ResultAvailable'.");
            }
            
            if (status == "Completed" && string.IsNullOrEmpty(result))
            {
                throw new InvalidOperationException("Kết quả xét nghiệm phải có khi trạng thái là 'Completed'.");
            }
            
            return await _repo.UpdateTestStatus(testId, status, result);
        }
    }
}
