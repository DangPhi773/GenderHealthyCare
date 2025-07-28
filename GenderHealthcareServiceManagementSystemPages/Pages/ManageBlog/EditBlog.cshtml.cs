using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ManageBlog
{
    public class EditBlogModel : PageModel
    {
        private readonly IBlogService _blogService;

        public EditBlogModel(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [BindProperty]
        public Blog Blog { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            Blog = await _blogService.GetByIdAsync(id);
            if (Blog == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingBlog = await _blogService.GetByIdAsync(Blog.BlogId);
            if (existingBlog == null)
                return NotFound();

            existingBlog.Title = Blog.Title;
            existingBlog.Content = Blog.Content;
            existingBlog.UpdatedAt = DateTime.UtcNow.AddHours(7);

            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdStr, out int userId))
            {
                existingBlog.AuthorId = userId;
            }

            await _blogService.UpdateAsync(existingBlog);

            TempData["Message"] = "Đã cập nhật blog thành công.";
            return RedirectToPage("/ManageBlog/AdminManagerBlogs");
        }

    }
}
