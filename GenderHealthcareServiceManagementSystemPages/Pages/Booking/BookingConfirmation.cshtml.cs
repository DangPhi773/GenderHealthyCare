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
        public ConsultantInfo ConsultantInfo { get; set; } = new ConsultantInfo();
        public string AppointmentDate { get; set; } = "#UNKNOWN";
        public string AppointmentTime { get; set; } = "#UNKNOWN";
        public string ConsultationStatus { get; set; } = "#UNKNOWN";
        //public string PatientName { get; set; } = "#UNKNOWN";
        //public string PhoneNumber { get; set; } = "#UNKNOWN";

        public async void OnGet()
        {
            BookingId = TempData["BookingId"]?.ToString() ?? "#UNKNOWN";
            //PatientName = TempData["PatientName"]?.ToString() ?? "Khách hàng";
            AppointmentDate = TempData["AppointmentDate"]?.ToString() ?? "Chưa rõ";
            AppointmentTime = TempData["AppointmentTime"]?.ToString() ?? "Chưa rõ";
            ConsultationStatus = TempData["ConsultationStatus"]?.ToString() ?? "Chưa rõ";
            int consultantId = TempData["ConsultantIdString"] != null ? int.Parse(TempData["ConsultantIdString"]?.ToString()!) : 0;
            if (consultantId != 0)
            {
                var consultantInfo = await _consultantInfoService.GetConsultantInfoByIdAsync(consultantId);
                if (consultantInfo != null)
                {
                    ConsultantInfo = consultantInfo;
                }
            }
        }
    }
}
