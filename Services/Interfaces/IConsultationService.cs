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
    Task<List<Consultation>> GetConsultationsByUser(int Id, string Role);
    Task<List<Consultation>> GetFilteredConsultations(int Id, string Role, string? status, string? name);
    Task<Consultation?> GetConsultationById(int consultationId);
    Task<bool> UpdateMeetingLinkAsync(int ConsultationId, string MeetingLink);
    Task UpdateStatus(Consultation consultation);
}
