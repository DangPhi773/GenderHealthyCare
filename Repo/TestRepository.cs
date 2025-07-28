using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;

namespace Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly TestDAO _dao;

        public TestRepository(TestDAO dao)
        {
            _dao = dao;
        }

        public Task<List<Test>> GetAllTest() => _dao.GetAllTest();
        public Task<Test?> GetTestById(int id) => _dao.GetTestById(id);
        public Task AddTest(Test test) => _dao.AddTest(test);
        public Task UpdateTest(Test test) => _dao.UpdateTest(test);
        public Task DeleteAsync(int id) => _dao.DeleteAsync(id);

        public Task<List<Test>> GetTestsByUserId(int id) => _dao.GetTestsByUserId(id);
        public Task<List<Test>> GetPendingTests() => _dao.GetPendingTests();
        
        public Task<List<Test>> GetScheduledTests() => _dao.GetScheduledTests();
        public async Task<bool> UpdateTestStatus(int testId, string status)
        {
            return await _dao.UpdateTestStatus(testId, status);
        }
        public async Task<bool> UpdateTestResultOrCancel(int testId, string result, string cancelReason)
        {
            return await _dao.UpdateTestResultOrCancel(testId, result, cancelReason );
        }
        public Task<bool> IsAppointmentTimeTestingConflict(int userId, DateTime selectedTime) => _dao.IsAppointmentTimeTestingConflict(userId, selectedTime);


    }
}
