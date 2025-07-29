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
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _dao;

        public UserRepository(UserDAO dao)
        {
            _dao = dao;
        }

        public Task<User?> GetUserById(int id) => _dao.GetUserById(id);
        public Task<List<User>> GetAllUsersAsync() => _dao.GetAllUsersAsync();
        public Task<List<User>> GetUsersByRoleAsync(string role) => _dao.GetUsersByRoleAsync(role);
        public Task<bool> AddUserAsync(User user) => _dao.AddUserAsync(user);
        public Task<bool> UpdateUser(User user) => _dao.UpdateUser(user);
        public async Task<int?> AddUserAndReturnIdAsync(User user)
        {
            return await _dao.AddUserAndReturnIdAsync(user);
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _dao.GetUsersAsync();
        }
        
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _dao.DeleteUserAsync(userId);
        }

    }
}