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
        Task<List<User>> GetUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
    }
}