using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerConsultation
{
    public class ShowUserConsultationsModel : PageModel
    {
        private readonly IConsultationService _consultationService;

        public ShowUserConsultationsModel(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        public List<Consultation> Consultations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Login");

            Consultations = await _consultationService.GetConsultationsByUserId(userId);
            return Page();
        }
    }

}
