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
using Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class EditModel : PageModel
    {
        private readonly ITestService _testService;
        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;
        private readonly IEmailService _emailService;

        public EditModel(ITestService testService, IUserService userService, IServiceService serviceService, IEmailService emailService)
        {
            _testService = testService;
            _userService = userService;
            _serviceService = serviceService;
            _emailService = emailService;
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
        private string ConvertStatusToVietnamese(string status)
        {
            return status switch
            {
                "Pending" => "Đang chờ",
                "Scheduled" => "Đã lên lịch",
                "Completed" => "Đã hoàn tất",
                "ResultAvailable" => "Có kết quả",
                "Cancelled" => "Đã hủy",
                _ => status
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"Updating Test: {Test.TestId}, Status: {Test.Status}, Result: {Test.Result}, CancelReason: {Test.CancelReason}");

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


            var updatedTest = await _testService.GetTestById(Test.TestId);
            var user = await _userService.GetUserById(updatedTest.UserId);

            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                try
                {
                    string vietStatus = ConvertStatusToVietnamese(Test.Status);
                    string subject = "Thông báo cập nhật xét nghiệm";
                    string message = $@"
            <div style='font-family:Arial,sans-serif;'>
                <h3 style='color:#2E86C1;'>Cập nhật xét nghiệm</h3>
                <p>Chào {user.FullName ?? "bạn"},</p>

                <p>Xét nghiệm của bạn đã được cập nhật với thông tin sau:</p>
                <ul>
                    <li><strong>Trạng thái:</strong> {vietStatus}</li>
                    {(Test.Result != null ? $"<li><strong>Kết quả:</strong> {Test.Result}</li>" : "")}
                    {(Test.CancelReason != null ? $"<li><strong>Lý do hủy:</strong> {Test.CancelReason}</li>" : "")}
                </ul>

                <p>Vui lòng đăng nhập hệ thống để xem chi tiết.</p>
                <p style='margin-top:20px;'>Trân trọng,<br/>Trung tâm y tế</p>
                <hr/>
                <small style='color:gray;'>Đây là email tự động, vui lòng không trả lời.</small>
            </div>";

                    await _emailService.SendEmailAsync(user.Email, subject, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                }
            }
            return RedirectToPage("./Index");


        }
    }
}
