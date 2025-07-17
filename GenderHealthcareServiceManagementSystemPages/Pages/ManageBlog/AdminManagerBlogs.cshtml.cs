using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.AdminManagerBlogs
{
    public class AdminManagerBlogsModel : PageModel
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public AdminManagerBlogsModel(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        public List<Blog> Blogs { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public bool ShowDeleted { get; set; }

        public string? Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Role = HttpContext.Session.GetString("Role");

            Blogs = await _blogService.GetAllAsync();
            Blogs = Blogs
                .Where(b => b.IsDeleted == ShowDeleted)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            foreach (var blog in Blogs)
            {
                blog.Author = await _userService.GetUserById(blog.AuthorId ?? 0);
            }

            return Page();
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                TempData["Message"] = "Bạn không có quyền xóa blog.";
                return RedirectToPage(new { ShowDeleted });
            }

            await _blogService.DeleteAsync(id);
            TempData["Message"] = "Đã xóa blog thành công.";
            return RedirectToPage(new { ShowDeleted });
        }

    }
}
