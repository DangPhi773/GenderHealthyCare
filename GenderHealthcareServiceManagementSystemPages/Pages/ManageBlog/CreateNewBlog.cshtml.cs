using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ManageBlog
{
    public class CreateNewBlogModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public CreateNewBlogModel(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        [BindProperty]
        public Blog Blog { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            // Kiểm tra đăng nhập
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Account/Login");

            // Có thể dùng userService để lấy thêm thông tin user nếu cần
            var user = await _userService.GetUserById(userId);
            if (user == null)
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra đăng nhập
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Account/Login");

            var user = await _userService.GetUserById(userId);
            if (user == null)
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid) return Page();

            Blog.AuthorId = userId;
            Blog.CreatedAt = DateTime.Now;
            Blog.UpdatedAt = DateTime.Now;
            Blog.IsDeleted = false;

            await _blogService.AddAsync(Blog);

            TempData["Message"] = "Tạo bài viết mới thành công.";
            return RedirectToPage("AdminManagerBlogs");
        }
    }
}
