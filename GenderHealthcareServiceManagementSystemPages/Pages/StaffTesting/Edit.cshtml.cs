using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class EditModel : PageModel
    {
        private readonly ITestService _testService;
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;

        public EditModel(ITestService testService, IUserService userService, IServiceService serviceService)
        {
            _testService = testService;
            _userService = userService;
            _serviceService = serviceService;
        }

        [BindProperty]
        public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var test = await _testService.GetTestById(id.Value);
            if (test == null) return NotFound();

            Test = test;
            ViewData["ServiceId"] = new SelectList(await _serviceService.GetAvailableServicesAsync(), "ServiceId", "Name");
            ViewData["UserId"] = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"Updating Test: {Test.TestId}, Status: {Test.Status}, Result: {Test.Result}, CancelReason: {Test.CancelReason}");

            //if (!ModelState.IsValid)
            //{
            //    ViewData["ServiceId"] = new SelectList(await _serviceService.GetAvailableServicesAsync(), "ServiceId", "Name");
            //    ViewData["UserId"] = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "Email");
            //    return Page();
            //}
            bool updated = await _testService.UpdateTestFields(
                Test.TestId,
                Test.Status,
                Test.Result,
                Test.CancelReason
            );

            if (!updated)
            {
                return NotFound(); 
            }

            return RedirectToPage("./Index");
        }
    }
    }
