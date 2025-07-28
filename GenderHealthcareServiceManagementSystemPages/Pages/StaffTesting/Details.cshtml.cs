using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class DetailsModel : PageModel
    {
        private readonly ITestService _testService;

        public DetailsModel(ITestService testService)
        {
            _testService = testService;
        }

        public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _testService.GetTestById(id.Value);
            if (test == null)
            {
                return NotFound();
            }
            Test = test;

            return Page();
        }
    }
}
