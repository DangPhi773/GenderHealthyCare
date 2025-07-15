using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO
    {
        private readonly GenderHealthcareContext _context;

        public UserDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await _context.Users
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
                return new List<User>();
            }
        }
        
        public async Task<User?> GetUserByEmail(string email)
        {
            Console.WriteLine($"[UserDAO][GetUserByEmail] Truy vấn người dùng với Email: {email}");
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                Console.WriteLine(user != null
                    ? $"[UserDAO][GetUserByEmail] Tìm thấy người dùng: {user.Username}, UserId: {user.UserId}"
                    : "[UserDAO][GetUserByEmail] Không tìm thấy người dùng.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserDAO][GetUserByEmail] Lỗi khi truy vấn: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetUserById(int id)
        {
            Console.WriteLine($"[UserDAO][GetUserById] Truy vấn người dùng với UserId: {id}");
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                Console.WriteLine(user != null
                    ? $"[UserDAO][GetUserById] Tìm thấy người dùng: {user.Username}, UserId: {user.UserId}"
                    : "[UserDAO][GetUserById] Không tìm thấy người dùng.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserDAO][GetUserById] Lỗi khi truy vấn: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            Console.WriteLine($"[UserDAO][GetUserByUsername] Truy vấn người dùng với Username: {username}");
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                Console.WriteLine(user != null
                    ? $"[UserDAO][GetUserByUsername] Tìm thấy người dùng: {user.Username}, UserId: {user.UserId}"
                    : "[UserDAO][GetUserByUsername] Không tìm thấy người dùng.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserDAO][GetUserByUsername] Lỗi khi truy vấn: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddUserAsync(User user)
        {
            Console.WriteLine($"[UserDAO][AddUserAsync] Thêm người dùng: {user.Username}, Email: {user.Email}");
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[UserDAO][AddUserAsync] Thêm người dùng thành công: {user.Username}, UserId: {user.UserId}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[UserDAO][AddUserAsync] Lỗi cơ sở dữ liệu khi thêm người dùng: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserDAO][AddUserAsync] Lỗi không xác định khi thêm người dùng: {ex.Message}");
                return false;
            }

        }
        public async Task<bool> UpdateUser(User user)
        {
            Console.WriteLine($"[UserDAO][UpdateUser] Cập nhật người dùng: {user.Username}, UserId: {user.UserId}");
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[UserDAO][UpdateUser] Cập nhật người dùng thành công: {user.Username}, UserId: {user.UserId}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[UserDAO][UpdateUser] Lỗi cơ sở dữ liệu khi cập nhật người dùng: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserDAO][UpdateUser] Lỗi không xác định khi cập nhật người dùng: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    user.IsDeleted = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }
    }
}