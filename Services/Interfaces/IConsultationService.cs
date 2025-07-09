using BusinessObjects.Models.Request;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces;
public interface IConsultationService
{
    Task<Consultation> CreateBookingAsync(BookingRequest request, int userId);
    Task<bool> IsSlotTakenAsync(int consultantId, DateTime appointmentTime);
    Task<List<string>> GetUnavailableSlotsAsync(DateTime date);
}
