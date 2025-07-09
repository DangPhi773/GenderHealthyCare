using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using DataAccessObjects;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserDAO _userDAO;

        [BindProperty]
        public User User { get; set; }

        public ProfileModel(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return RedirectToPage("/Login");
            }
            User = await _userDAO.GetUserById(id);
            if (User == null)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
            {
                return RedirectToPage("/Login");
            }

            var existingUser = await _userDAO.GetUserById(id);
            if (existingUser == null)
            {
                return RedirectToPage("/Login");
            }

            // Cập nhật thông tin từ User đã bind
            existingUser.FullName = User.FullName;
            existingUser.Email = User.Email;
            existingUser.Phone = User.Phone;

            // Chuyển đổi giá trị giới tính từ tiếng Việt sang tiếng Anh
            existingUser.Gender = User.Gender switch
            {
                "Nam" => "Male",
                "Nữ" => "Female",
                "Khác" => "Other",
                _ => existingUser.Gender // Giữ nguyên nếu không khớp
            };

            if (User.Dob.HasValue)
            {
                existingUser.Dob = User.Dob;
            }

            // Gọi UpdateUser để lưu
            bool isUpdated = await _userDAO.UpdateUser(existingUser);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                return RedirectToPage("/Profile");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Cập nhật thông tin thất bại. Vui lòng kiểm tra lại log hoặc liên hệ hỗ trợ.");
                Console.WriteLine($"[ProfileModel][OnPostAsync] Cập nhật thất bại cho UserId: {id}");
                return Page();
            }
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login"); 
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}