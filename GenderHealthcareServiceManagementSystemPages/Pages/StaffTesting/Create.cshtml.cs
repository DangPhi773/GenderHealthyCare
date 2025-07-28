using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class CreateModel : PageModel
    {
        private readonly ITestService _testService;
        private readonly IServiceService _serviceService;
        private readonly IUserService _userService;

        public CreateModel(ITestService testService, IServiceService serviceService, IUserService userService)
        {
            _testService = testService;
            _serviceService = serviceService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var services = await _serviceService.GetAvailableServicesAsync();
            var users = await _userService.GetAllUsersAsync();

            ViewData["ServiceId"] = new SelectList(services, "ServiceId", "Name");
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");

            return Page();
        }

        [BindProperty]
        public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var services = await _serviceService.GetAvailableServicesAsync();
                var users = await _userService.GetAllUsersAsync();

                ViewData["ServiceId"] = new SelectList(services, "ServiceId", "Name");
                ViewData["UserId"] = new SelectList(users, "UserId", "Email");

                Test.Status = "Pending";
                Test.Result = null;
                Test.CancelReason = null;
            }
            await _testService.AddTest(Test);

            return RedirectToPage("./Index");
        }
    }
}
