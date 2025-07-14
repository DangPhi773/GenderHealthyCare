using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Consultant
{
    public class ProfileModel : PageModel
    {
        private readonly IConsultantInfoService _consultantService;

        public ProfileModel(IConsultantInfoService consultantService)
        {
            _consultantService = consultantService;
        }

        public ConsultantInfo? ConsultantInfo { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            Console.WriteLine($"[ConsultantProfile] UserId in session: {userId}");

            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(userId) || role != "Consultant" || !int.TryParse(userId, out int consultantId))
            {
                return RedirectToPage("/Login");
            }

            ConsultantInfo = await _consultantService.GetConsultantInfoByIdAsync(consultantId);
            return Page();
        }
    }
}
