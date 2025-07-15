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
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public Task<User?> GetUserById(int id) => _repo.GetUserById(id);
        public async Task<List<User>> GetUsersAsync()
        {
            return await _repo.GetUsersAsync();
        }
        
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _repo.DeleteUserAsync(userId);
        }
        
    }
}

