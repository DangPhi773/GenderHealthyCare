using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.CustomerConsultation
{
    public class ShowUserConsultationsModel : PageModel
    {
        private readonly IConsultationService _consultationService;
        private readonly IFeedbackService _feedbackService;

        public ShowUserConsultationsModel(
            IConsultationService consultationService,
            IFeedbackService feedbackService)
        {
            _consultationService = consultationService;
            _feedbackService = feedbackService;
        }

        public List<Consultation> Consultations { get; set; } = new();
        public Dictionary<int, bool> FeedbackStatus { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Login");

            Consultations = await _consultationService.GetConsultationsByUserId(userId);

            foreach (var consultation in Consultations)
            {
                var feedback = await _feedbackService.GetFeedbackByConsultationIdAsync(userId, consultation.ConsultationId);
                FeedbackStatus[consultation.ConsultationId] = feedback != null;
            }


            return Page();
        }

        public IActionResult OnPostSetConsultationId(int consultationId)
        {
            HttpContext.Session.SetInt32("SelectedConsultationId", consultationId);
            return RedirectToPage("FeedbackConsultation");
        }
    }

}
