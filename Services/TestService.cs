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
    }
}
