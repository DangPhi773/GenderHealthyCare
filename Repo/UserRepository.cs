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
        
        public async Task<List<User>> GetUsersAsync()
        {
            return await _dao.GetUsersAsync();
        }
    }
}