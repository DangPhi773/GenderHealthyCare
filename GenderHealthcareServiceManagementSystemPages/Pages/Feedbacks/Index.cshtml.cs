using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;
using DataAccessObjects;
using BusinessObjects.ViewModels;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Feedbacks
{
    public class IndexModel : PageModel
    {
        private readonly IFeedbackService _iFeedbackService;

        public IndexModel(IFeedbackService iFeedbackService)
        {
            _iFeedbackService = iFeedbackService;
        }

        public FeedbackStats FeedbackStats { get; set; }
        public List<FeedbackDisplay> FeedbackDisplays { get; set; }

        public string? CurrentRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int? consultantId, int? serviceId)
        {
            //consultantId = 4;
            serviceId = 1;
            CurrentRole = "Admin";
            if (consultantId.HasValue)
            {
                (FeedbackStats, FeedbackDisplays) = await _iFeedbackService.GetFeedbacksById(consultantId.Value, "consultant");
            }
            else if (serviceId.HasValue)
            {
                (FeedbackStats, FeedbackDisplays) = await _iFeedbackService.GetFeedbacksById(serviceId.Value, "service");
            }
            else
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _iFeedbackService.DeleteFeedback(id);
            return RedirectToPage(); // Reload trang
        }

    }
}
