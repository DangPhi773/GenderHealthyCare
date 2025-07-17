using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerBlogs;

public class CustomerBlogsPage : PageModel
{
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;

    public CustomerBlogsPage(IBlogService blogService, IUserService userService)
    {
        _blogService = blogService;
        _userService = userService;
    }

    public List<Blog> Blogs { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public bool ShowDeleted { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
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
}