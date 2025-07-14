using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Consultations
{
    public class IndexModel : PageModel
    {
        private readonly IConsultationService _consultationService;
        public IndexModel(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ConsultantName { get; set; }
        public IList<Consultation> Consultation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            int userId = 4;
            string role = "Staff";
            if(String.IsNullOrEmpty(StatusFilter) && String.IsNullOrEmpty(ConsultantName))
            {
                Consultation = await _consultationService.GetConsultationsByUser(userId, role);
            }
            else
            {
                Consultation = await _consultationService.GetFilteredConsultations(userId, role, StatusFilter, ConsultantName);
                
            }
            foreach (var consultation in Consultation)
            {
                await _consultationService.UpdateStatus(consultation);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateLinkAsync(int ConsultationId, string MeetingLink)
        {
            var success = await _consultationService.UpdateMeetingLinkAsync(ConsultationId, MeetingLink);

            if (!success)
            {
                return NotFound();
            }
            return RedirectToPage();
        }
    }
}
