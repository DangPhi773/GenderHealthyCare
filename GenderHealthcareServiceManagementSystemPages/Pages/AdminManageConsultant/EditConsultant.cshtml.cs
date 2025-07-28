using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.AdminManageConsultant
{
    public class EditConsultantModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConsultantInfoService _consultantInfoService;

        public EditConsultantModel(IUserService userService, IConsultantInfoService consultantInfoService)
        {
            _userService = userService;
            _consultantInfoService = consultantInfoService;
        }

        [BindProperty]
        public User User { get; set; } = new();

        [BindProperty]
        public ConsultantInfo ConsultantInfo { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }
            var user = await _userService.GetUserById(id);
            var consultantInfo = await _consultantInfoService.GetConsultantInfoByIdAsync(id);

            //if (user == null || consultantInfo == null)
            //{
            //    return NotFound();
            //}
            if (user == null)
            {
                return NotFound();
            }

            User = user;
            ConsultantInfo = consultantInfo ?? new ConsultantInfo
            {
                ConsultantId = id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var existingUser = await _userService.GetUserById(id);
            var existingInfo = await _consultantInfoService.GetConsultantInfoByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FullName = User.FullName;
            existingUser.Email = User.Email;
            existingUser.Dob = User.Dob;
            await _userService.UpdateUser(existingUser);

            if (existingInfo == null)
            {
                ConsultantInfo.ConsultantId = id;
                ConsultantInfo.CreatedAt = DateTime.Now;
                await _consultantInfoService.CreateConsultantInfoAsync(ConsultantInfo);
            }
            else
            {
                existingInfo.Specialization = ConsultantInfo.Specialization;
                existingInfo.ExperienceYears = ConsultantInfo.ExperienceYears;
                existingInfo.Qualifications = ConsultantInfo.Qualifications;
                await _consultantInfoService.UpdateConsultantInfoAsync(existingInfo);
            }

            TempData["Message"] = "Đã cập nhật thông tin tư vấn viên thành công.";
            return RedirectToPage("ManageConsultant");
        }

    }
}
