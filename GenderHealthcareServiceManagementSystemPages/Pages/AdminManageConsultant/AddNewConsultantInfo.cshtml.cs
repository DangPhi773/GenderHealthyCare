using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GenderHealthcareServiceManagementSystemPages.Pages.AdminManageConsultant
{
    public class AddNewConsultantInfoModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConsultantInfoService _consultantInfoService;

        public AddNewConsultantInfoModel(IUserService userService, IConsultantInfoService consultantInfoService)
        {
            _userService = userService;
            _consultantInfoService = consultantInfoService;
        }

        [BindProperty]
        public User User { get; set; } = new();

        [BindProperty]
        public ConsultantInfo ConsultantInfo { get; set; } = new();

        [BindProperty]
        public IFormFile? ProfileImage { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var key in ModelState.Keys)
            //    {
            //        foreach (var error in ModelState[key].Errors)
            //        {
            //            Console.WriteLine($"❌ ModelState Error: {key} - {error.ErrorMessage}");
            //        }
            //    }

            //    Message = "Vui lòng kiểm tra lại thông tin.";
            //    return Page();
            //}

            User.Role = "Consultant";
            User.CreatedAt = DateTime.Now;
            User.UpdatedAt = DateTime.Now;
            User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(User.PasswordHash);


            var userId = await _userService.AddUserAndReturnIdAsync(User);
            if (userId == null || userId == 0)
            {
                Message = "Không thể thêm người dùng.";
                return Page();
            }

            ConsultantInfo.ConsultantId = userId.Value;
            ConsultantInfo.CreatedAt = DateTime.Now;

            if (ProfileImage != null)
            {
                using var ms = new MemoryStream();
                await ProfileImage.CopyToAsync(ms);
                ConsultantInfo.ProfileImage = ms.ToArray();
            }

            var success = await _consultantInfoService.AddConsultantInfoAsync(ConsultantInfo);
            if (!success)
            {
                Message = "Không thể thêm thông tin tư vấn viên.";
                return Page();
            }

            TempData["Message"] = "Thêm tư vấn viên thành công.";
            return RedirectToPage("ManageConsultant");
        }
    }
}
