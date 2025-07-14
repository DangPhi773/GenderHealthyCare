using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin;

public class ManageUser : PageModel
{
    private readonly IUserService _userService;

    public ManageUser(IUserService userService)
    {
        _userService = userService;
    }

    public List<User> Users { get; set; }

    public async Task OnGetAsync()
    {
        // Fetch all users
        Users = await _userService.GetUsersAsync();
    }

    /*public async Task<IActionResult> OnDeleteUserAsync(int id)
    {
        // Delete user by id
        var result = await _userService.DeleteUserAsync(id);
        if (result)
        {
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }*/
}