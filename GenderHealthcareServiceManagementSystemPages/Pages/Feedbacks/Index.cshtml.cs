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
        public string FeedbackType { get; set; } = "Service";

        public string CurrentRole { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(string? type, bool showDeleted = false)
        {
            CurrentRole = HttpContext.Session.GetString("Role") ?? "";
            FeedbackType = type ?? "Service";
            if (FeedbackType == "Consultant")
            {
                (FeedbackStats, FeedbackDisplays) = await _iFeedbackService.GetFeedbacksByTask("consultant", showDeleted);
            }
            else if (FeedbackType == "Service")
            {
                (FeedbackStats, FeedbackDisplays) = await _iFeedbackService.GetFeedbacksByTask("service", showDeleted);
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
