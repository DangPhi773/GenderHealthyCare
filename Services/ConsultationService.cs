using BusinessObjects.Models.Request;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;
public class ConsultationService(IConsultationRepository repo) : IConsultationService
{
    private readonly IConsultationRepository _repo = repo;

    public async Task<Consultation> CreateBookingAsync(BookingRequest request, int userId)
    {
        var booking = new Consultation
        {
            UserId = userId,
            ConsultantId = int.Parse(request.SelectedConsultant),
            AppointmentTime = DateTime.Parse($"{request.AppointmentDate:yyyy-MM-dd} {request.AppointmentTime}"),
            Status = "Pending",
            Notes = request.Notes,
            CreatedAt = DateTime.Now
        };

        var createdBooking = await _repo.AddAsync(booking);
        return createdBooking;
    }
    public async Task<bool> IsSlotTakenAsync(int consultantId, DateTime appointmentTime)
    {
        return await _repo.IsSlotAvailableAsync(consultantId, appointmentTime) == false;
    }
    public async Task<List<string>> GetUnavailableSlotsAsync(DateTime date)
    {
        return await _repo.GetUnavailableSlotsAsync(date);
    }

    public async Task<Consultation?> GetConsultationById(int consultationId)
    {
        return await _repo.GetConsultationById(consultationId);
    }

    public async Task<List<Consultation>> GetConsultationsByUser(int Id, string Role)
    {
        return await _repo.GetConsultationsByUser(Id, Role);
    }

    public async Task<List<Consultation>> GetFilteredConsultations(int userId, string userRole, string? status, string? name)
    {
        return await _repo.GetFilteredConsultations(userId, userRole, status, name);
    }

    public async Task<bool> UpdateMeetingLinkAsync(int ConsultationId, string MeetingLink)
    {
        var consultation = await _repo.GetConsultationById(ConsultationId);
        if (consultation == null)
        {
            return false;
        }

        if (consultation.Status == "Pending"
            || (consultation.Status == "Confirmed"
            && consultation.AppointmentTime.Date > DateTime.Today))
        {
            consultation.MeetingLink = MeetingLink;

            if (consultation.Status == "Pending")
                consultation.Status = "Confirmed";

            await _repo.UpdateConsultation(consultation);
            return true;
        }
        return false;
    }

    public async Task UpdateStatus(Consultation consultation)
    {
        var now = DateTime.Now;
        if (consultation.Status == "Pending" && consultation.AppointmentTime < now)
        {
            consultation.Status = "Cancelled";
        }
        else if (consultation.Status == "Confirmed" && now > consultation.AppointmentTime.AddMinutes(30))
        {
            consultation.Status = "Completed";
        }
        await _repo.UpdateConsultation(consultation);
    }
}
