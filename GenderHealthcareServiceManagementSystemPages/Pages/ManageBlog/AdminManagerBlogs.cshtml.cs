using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ManageBlog
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

        public async Task<IActionResult> OnGetAsync()
        {
            Blogs = await _blogService.GetAllAsync();

            foreach (var blog in Blogs)
            {
                blog.Author = await _userService.GetUserById(blog.AuthorId ?? 0);
            }

            Blogs = Blogs.OrderByDescending(b => b.CreatedAt).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _blogService.DeleteAsync(id);
            TempData["Message"] = "Đã xóa blog thành công.";
            return RedirectToPage();
        }
    }
}
