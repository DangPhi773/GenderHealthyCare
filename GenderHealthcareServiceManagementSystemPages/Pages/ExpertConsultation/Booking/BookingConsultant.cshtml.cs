using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class BookingConsultantModel(IConsultantInfoService consultantInfoService, IConsultationService consultationService, IHubContext<SignalRServer> hubContext, IEmailService emailService) : PageModel
    {
        private readonly IConsultantInfoService _consultantInfoService = consultantInfoService ?? throw new ArgumentNullException(nameof(consultantInfoService));
        private readonly IConsultationService _consultationService = consultationService ?? throw new ArgumentNullException(nameof(consultationService));
        private readonly IHubContext<SignalRServer> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

        [BindProperty]
        public BookingRequest Booking { get; set; } = new();

        public List<ConsultantInfo> ConsultantInfos { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt lịch hẹn.";
                return RedirectToPage("/Login", new { returnUrl = "/ExpertConsultation/Booking/BookingConsultant" });
            }

            // Initialize page
            ConsultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ConsultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();
                return Page();
            }

            var selectedDateTime = DateTime.Parse($"{Booking.AppointmentDate:yyyy-MM-dd} {Booking.AppointmentTime}");
            if (await _consultationService.IsSlotTakenAsync(int.Parse(Booking.SelectedConsultant), selectedDateTime))
            {
                ModelState.AddModelError("", "Khung giờ này đã được đặt. Vui lòng chọn giờ khác.");
                ConsultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();// Load lại
                return Page();
            }

            //Save to database
            var createdBooking = await _consultationService.CreateBookingAsync(Booking, GetCurrentUserId());

            await _emailService.SendEmailAsync(
                Booking.Email,
                "Xác nhận đặt lịch",
                "<p>Lịch khám của bạn đã được xác nhận.</p>"
            );
            // Redirect
            //await _hubContext.Clients.All.SendAsync("AppointmentCreated");
            TempData["BookingId"] = createdBooking.ConsultationId;
            TempData["AppointmentTime"] = Booking.AppointmentTime;
            TempData["AppointmentDate"] = Booking.AppointmentDate.ToString("dddd, dd/MM/yyyy");
            TempData["ConsultantIdString"] = Booking.SelectedConsultant;
            TempData["ConsultationStatus"] = "Đợi xét duyệt";
            return RedirectToPage("/ExpertConsultation/Booking/BookingConfirmation");
        }

        public async Task<JsonResult> OnGetUnavailableSlotsAsync(string date)
        {
            var parsedDate = DateTime.Parse(date);
            var unavailableSlots = await _consultationService.GetUnavailableSlotsAsync(parsedDate);
            return new JsonResult(unavailableSlots);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(HttpContext.Session.GetString("UserId")!);
        }

        // Helper method to get gender display text
        public string GetGenderDisplayText(string? gender)
        {
            return gender?.ToLower() switch
            {
                "female" => "Nữ",
                "male" => "Nam",
                _ => "Không xác định"
            };
        }

        // Add method to check if consultant has image
        public bool HasProfileImage(ConsultantInfo consultantInfo)
        {
            return consultantInfo?.ProfileImage != null &&
                   consultantInfo.ProfileImage.Length > 0;
        }

        public async Task<IActionResult> OnGetConsultantImageAsync(int consultantId)
        {
            try
            {
                var consultantInfo = await _consultantInfoService.GetConsultantInfoByIdAsync(consultantId);

                if (consultantInfo?.ProfileImage != null && consultantInfo.ProfileImage.Length > 0)
                {
                    string contentType = "image/jpeg";
                    return File(consultantInfo.ProfileImage, contentType);
                }

                // Fallback to default images if no profile image
                return GetDefaultImage(consultantInfo?.Consultant?.Gender);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error loading consultant image for ID {consultantId}: {ex.Message}");
                // Return a more visible default image on error
                return GetDefaultImage("unknown"); // Use a generic default if gender is also unknown
            }
        }

        private IActionResult GetDefaultImage(string? gender)
        {
            // Use base64 encoded SVG for default images directly in C#
            string defaultImageSvg;
            if (gender?.ToLower() == "female")
            {
                defaultImageSvg = @"<svg xmlns='http://www.w3.org/2000/svg' width='80' height='80' viewBox='0 0 80 80'>
                    <circle cx='40' cy='40' r='40' fill='#ec4899'/>
                    <text x='40' y='50' text-anchor='middle' fill='white' font-size='30' font-family='Arial'>👩‍⚕️</text>
                </svg>";
            }
            else if (gender?.ToLower() == "male")
            {
                defaultImageSvg = @"<svg xmlns='http://www.w3.org/2000/svg' width='80' height='80' viewBox='0 0 80 80'>
                    <circle cx='40' cy='40' r='40' fill='#3b82f6'/>
                    <text x='40' y='50' text-anchor='middle' fill='white' font-size='30' font-family='Arial'>👨‍⚕️</text>
                </svg>";
            }
            else // Generic fallback
            {
                defaultImageSvg = @"<svg xmlns='http://www.w3.org/2000/svg' width='80' height='80' viewBox='0 0 80 80'>
                    <circle cx='40' cy='40' r='40' fill='#d1d5db'/>
                    <text x='40' y='50' text-anchor='middle' fill='white' font-size='30' font-family='Arial'>👤</text>
                </svg>";
            }

            var svgBytes = System.Text.Encoding.UTF8.GetBytes(defaultImageSvg);
            return File(svgBytes, "image/svg+xml");
        }
    }
}
