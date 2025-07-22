using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories;
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
        public Task<List<User>> GetAllUsersAsync() => _repo.GetAllUsersAsync();

        public Task<List<User>> GetUsersByRoleAsync(string role) => _repo.GetUsersByRoleAsync(role);
        public Task<bool> AddUserAsync(User user) => _repo.AddUserAsync(user);

        public Task<bool> UpdateUser(User user) => _repo.UpdateUser(user);
        public async Task<int?> AddUserAndReturnIdAsync(User user)
        {
            return await _repo.AddUserAndReturnIdAsync(user);
        }


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

