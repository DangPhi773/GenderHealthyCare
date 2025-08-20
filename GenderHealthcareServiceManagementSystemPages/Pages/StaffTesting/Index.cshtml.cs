using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;
using Services;

namespace GenderHealthcareServiceManagementSystemPages.Pages.StaffTesting
{
    public class IndexModel : PageModel
    {
        private readonly ITestService _testService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public IndexModel(ITestService service, IUserService userService, IEmailService emailService)
        {
            _testService = service;
            _userService = userService;
            _emailService = emailService;
        }

        public IList<Test> Test { get; set; } = default!;
        public Test? TestDetails { get; set; }

        [BindProperty]
        public Test EditTest { get; set; } = new Test();

        public async Task OnGetAsync()
        {
            try
            {
                Test = await _testService.GetAllTest() ?? new List<Test>();
            }
            catch (Exception ex)
            {
                Test = new List<Test>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Debug logging
                Console.WriteLine($"OnPostAsync called");
                Console.WriteLine($"EditTest.TestId: {EditTest.TestId}");
                Console.WriteLine($"EditTest.AppointmentTime: {EditTest.AppointmentTime}");
                Console.WriteLine($"EditTest.Status: {EditTest.Status}");
                Console.WriteLine($"EditTest.Result: {EditTest.Result}");
                Console.WriteLine($"EditTest.CancelReason: {EditTest.CancelReason}");
                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

                // Validate appointment time
                if (EditTest.AppointmentTime < DateTime.Now)
                {
                    ModelState.AddModelError("EditTest.AppointmentTime", "Thời gian hẹn không thể là thời gian trong quá khứ");
                    Test = await _testService.GetAllTest() ?? new List<Test>();
                    return Page();
                }

                var currentTest = await _testService.GetTestById(EditTest.TestId);
                if (currentTest == null)
                {
                    Test = await _testService.GetAllTest() ?? new List<Test>();
                    return NotFound();
                }

                // Update the test with form data
                currentTest.AppointmentTime = EditTest.AppointmentTime;
                currentTest.Status = EditTest.Status;
                currentTest.Result = EditTest.Result;
                currentTest.CancelReason = EditTest.CancelReason;

                try
                {
                    await _testService.UpdateTest(currentTest);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin xét nghiệm");
                    Test = await _testService.GetAllTest() ?? new List<Test>();
                    return Page();
                }

                // Send email notification
                var user = await _userService.GetUserById(currentTest.UserId);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    try
                    {
                        string vietStatus = ConvertStatusToVietnamese(currentTest.Status);
                        string subject = "Thông báo cập nhật xét nghiệm";
                        string message = $@"
            <div style='font-family:Arial,sans-serif;'>
                <h3 style='color:#2E86C1;'>Cập nhật xét nghiệm</h3>
                <p>Chào {user.FullName ?? "bạn"},</p>

                <p>Xét nghiệm của bạn đã được cập nhật với thông tin sau:</p>
                <ul>
                    <li><strong>Thời gian hẹn:</strong> {currentTest.AppointmentTime.ToString("dd/MM/yyyy HH:mm")}</li>
                    <li><strong>Trạng thái:</strong> {vietStatus}</li>
                    {(currentTest.Result != null ? $"<li><strong>Kết quả:</strong> {currentTest.Result}</li>" : "")}
                    {(currentTest.CancelReason != null ? $"<li><strong>Lý do hủy:</strong> {currentTest.CancelReason}</li>" : "")}
                </ul>

                <p>Vui lòng đăng nhập hệ thống để xem chi tiết.</p>
                <p style='margin-top:20px;'>Trân trọng,<br/>Trung tâm y tế</p>
                <hr/>
                <small style='color:gray;'>Đây là email tự động, vui lòng không trả lời.</small>
            </div>
                        ";

                        await _emailService.SendEmailAsync(user.Email, subject, message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin xét nghiệm");
                Test = await _testService.GetAllTest() ?? new List<Test>();
                return Page();
            }
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
    }
}

