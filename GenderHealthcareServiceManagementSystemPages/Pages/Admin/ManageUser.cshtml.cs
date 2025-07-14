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

        Users = await _userService.GetUsersAsync();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync(int id)
    {
        var result = await _userService.DeleteUserAsync(id);

        if (result)
        {
            TempData["SuccessMessage"] = "User deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete user.";
        }
        return RedirectToPage("ManageUsers");
    }
}