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

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly IClinicService _iClinicService;

        public DetailsModel(IClinicService iClinicService)
        {
            _iClinicService = iClinicService;
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _iClinicService.GetByIdAsync((int)id);
            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                Clinic = clinic;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateClinicAsync(string action)
        {
            if (action == "update")
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var success = await _iClinicService.UpdateAsync(Clinic);

                if (!success)
                {
                    return NotFound();
                }
                return RedirectToPage(new { id = Clinic.ClinicId });
            }

            if (action == "delete")
            {
                var deleted = await _iClinicService.DeleteAsync(Clinic.ClinicId);
                if (!deleted)
                    return NotFound();

                return RedirectToPage("Index");
            }
            return Page();
        }

    }
}
