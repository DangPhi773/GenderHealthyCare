using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> RegisterAsync(User user);
        Task<User?> LoginAsync(string username, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ConfirmEmailAsync(User user);
    }
}
