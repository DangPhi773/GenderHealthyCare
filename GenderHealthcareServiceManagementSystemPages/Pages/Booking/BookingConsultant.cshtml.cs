using BusinessObjects.Models;
using BusinessObjects.Models.Request;
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
    public class BookingConsultantModel(IConsultantInfoService consultantInfoService, IConsultationService consultationService, IHubContext<SignalRServer> hubContext) : PageModel
    {
        private readonly IConsultantInfoService _consultantInfoService = consultantInfoService ?? throw new ArgumentNullException(nameof(consultantInfoService));
        private readonly IConsultationService _consultationService = consultationService ?? throw new ArgumentNullException(nameof(consultationService));
        private readonly IHubContext<SignalRServer> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));

        [BindProperty]
        public BookingRequest Booking { get; set; } = new ();
        public List<ConsultantInfo> ConsultantInfos { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt lịch hẹn.";
                return RedirectToPage("/Login");
            }
            // Initialize page
            ConsultantInfos = await _consultantInfoService.GetAllConsultantInfosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var selectedDateTime = DateTime.Parse($"{Booking.AppointmentDate:yyyy-MM-dd} {Booking.AppointmentTime}");
            if (await _consultationService.IsSlotTakenAsync(int.Parse(Booking.SelectedConsultant), selectedDateTime))
            {
                ModelState.AddModelError("", "Khung giờ này đã được đặt. Vui lòng chọn giờ khác.");
                ConsultantInfos =  await _consultantInfoService.GetAllConsultantInfosAsync();// Load lại
                return Page();
            }
            //Save to database
            var createdBooking = await _consultationService.CreateBookingAsync(Booking, GetCurrentUserId());

            // Redirect
            //await _hubContext.Clients.All.SendAsync("AppointmentCreated");
            TempData["BookingId"] = createdBooking.ConsultationId;
            TempData["AppointmentTime"] = Booking.AppointmentTime;
            TempData["AppointmentDate"] = Booking.AppointmentDate.ToString("dddd, dd/MM/yyyy");
            TempData["ConsultantIdString"] = Booking.SelectedConsultant;
            TempData["ConsultationStatus"] = "Đợi xét duyệt";
            return RedirectToPage("/Booking/BookingConfirmation");
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
    }

}
