using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            Console.WriteLine($"[AccountService][RegisterAsync] Gọi RegisterAsync cho Username: {user.Username}");
            return await _accountRepository.RegisterAsync(user);
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            Console.WriteLine($"[AccountService][LoginAsync] Gọi LoginAsync với Username: {username}");
            var user = await _accountRepository.LoginAsync(username, password);
            if (user != null)
            {
                Console.WriteLine($"[AccountService][LoginAsync] Tìm thấy người dùng: {user.Username}, UserId: {user.UserId}");
            }
            else
            {
                Console.WriteLine("[AccountService][LoginAsync] Không tìm thấy người dùng hoặc mật khẩu không đúng.");
            }
            return user;
        }
    }
}