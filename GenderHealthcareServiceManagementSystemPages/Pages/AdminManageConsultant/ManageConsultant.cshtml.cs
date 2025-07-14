using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.AdminManageConsultant
{
    public class ManageConsultantModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConsultantInfoService _consultantInfoService;

        public ManageConsultantModel(IUserService userService, IConsultantInfoService consultantInfoService)
        {
            _userService = userService;
            _consultantInfoService = consultantInfoService;
        }

        public List<(User User, ConsultantInfo ConsultantInfo)> Consultants { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userService.GetUsersByRoleAsync("Consultant");
            var consultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();

            Consultants = users.Select(user =>
            {
                var info = consultantInfos.FirstOrDefault(c => c.ConsultantId == user.UserId)
                          ?? new ConsultantInfo { ConsultantId = user.UserId };
                return (user, info);
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var consultant = await _userService.GetUserById(id);
            if (consultant != null && consultant.Role == "Consultant")
            {
                await _consultantInfoService.DeleteConsultantInfoAsync(id);
                await _userService.DeleteUserAsync(id);

                TempData["Message"] = "Xóa tư vấn viên thành công.";
            }

            return RedirectToPage();
        }
    }
}
