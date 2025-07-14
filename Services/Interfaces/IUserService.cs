using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<int?> AddUserAndReturnIdAsync(User user);

    }
}