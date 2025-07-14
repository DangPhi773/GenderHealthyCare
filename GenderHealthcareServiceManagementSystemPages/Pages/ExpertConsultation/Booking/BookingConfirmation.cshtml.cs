using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Reflection.Metadata;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Booking
{
    public class BookingConfirmationModel(IConsultantInfoService consultantInfoService) : PageModel
    {
        private readonly IConsultantInfoService _consultantInfoService = consultantInfoService;
        public string BookingId { get; set; } = "#UNKNOWN";
        public ConsultantInfo ConsultantInfo { get; set; } = new ();
        public string AppointmentDate { get; set; } = "#UNKNOWN";
        public string AppointmentTime { get; set; } = "#UNKNOWN";
        public string ConsultationStatus { get; set; } = "#UNKNOWN";

        public async Task OnGet()
        {
            BookingId = TempData["BookingId"]?.ToString() ?? "#UNKNOWN";
            AppointmentDate = TempData["AppointmentDate"]?.ToString() ?? "Chưa rõ";
            AppointmentTime = TempData["AppointmentTime"]?.ToString() ?? "Chưa rõ";
            ConsultationStatus = TempData["ConsultationStatus"]?.ToString() ?? "Chưa rõ";
            int consultantId = TempData["ConsultantIdString"] != null ? int.Parse(TempData["ConsultantIdString"]?.ToString()!) : 0;
            if (consultantId != 0)
            {
                var consultantInfo = await _consultantInfoService.GetConsultantInfoByIdAsync(consultantId) ?? null;
                if (consultantInfo != null)
                {
                    ConsultantInfo = consultantInfo;
                }
            }
        }
    }
}
