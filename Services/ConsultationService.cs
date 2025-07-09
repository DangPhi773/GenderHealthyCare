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
}
