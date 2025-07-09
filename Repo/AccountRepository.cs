using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserDAO _userDAO;

        public AccountRepository(GenderHealthcareContext context)
        {
            _userDAO = new UserDAO(context);
        }

        public async Task<bool> RegisterAsync(User user)
        {
            Console.WriteLine($"[AccountRepository][RegisterAsync] Kiểm tra trùng lặp cho Username: {user.Username}, Email: {user.Email}");
            if (await _userDAO.GetUserByUsername(user.Username) != null ||
                await _userDAO.GetUserByEmail(user.Email) != null)
            {
                Console.WriteLine("[AccountRepository][RegisterAsync] Tên đăng nhập hoặc email đã tồn tại.");
                return false;
            }

            Console.WriteLine("[AccountRepository][RegisterAsync] Gán giá trị mặc định và mã hóa mật khẩu...");
            if (string.IsNullOrEmpty(user.Role) ||
                !new[] { "Guest", "Customer", "Consultant", "Staff", "Manager", "Admin" }.Contains(user.Role))
            {
                user.Role = "Customer";
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            Console.WriteLine("[AccountRepository][RegisterAsync] Thêm người dùng vào cơ sở dữ liệu...");
            return await _userDAO.AddUserAsync(user);
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            Console.WriteLine($"[AccountRepository][LoginAsync] Tìm người dùng với Username: {username}");
            var user = await _userDAO.GetUserByUsername(username);
            if (user == null)
            {
                Console.WriteLine("[AccountRepository][LoginAsync] Không tìm thấy người dùng.");
                return null;
            }

            Console.WriteLine($"[AccountRepository][LoginAsync] Tìm thấy người dùng: {user.Username}, UserId: {user.UserId}. Đang xác minh mật khẩu...");
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                Console.WriteLine("[AccountRepository][LoginAsync] Mật khẩu khớp. Đăng nhập thành công.");
                return user;
            }

            Console.WriteLine("[AccountRepository][LoginAsync] Mật khẩu không khớp.");
            return null;
        }
    }
}