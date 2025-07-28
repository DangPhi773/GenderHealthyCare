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
    public class IndexModel : PageModel
    {
        private readonly ITestService _testService;

        public IndexModel(ITestService service)
        {
            _testService = service;
        }

        public IList<Test> Test { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_testService.GetAllTest != null)
            {
                Test = await _testService.GetAllTest();
            }
            else
            {
                Test = new List<Test>();

            }
        }

    }
}

