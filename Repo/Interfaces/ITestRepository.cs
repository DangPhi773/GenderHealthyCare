using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface ITestRepository
    {
        Task<List<Test>> GetAllTest();
        Task<Test?> GetTestById(int id);
        Task AddTest(Test test);
        Task UpdateTest(Test test);
        Task DeleteAsync(int id);
        Task<List<Test>> GetTestsByUserId(int id);
        Task<List<Test>> GetPendingTests();
        Task<List<Test>> GetScheduledTests();
        Task<bool> UpdateTestStatus(int testId, string status, string result = null);
    }
}