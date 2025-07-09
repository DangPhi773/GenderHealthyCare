using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects;
public class ConsultationDAO(GenderHealthcareContext context)
{
    private readonly GenderHealthcareContext _context = context;
    public async Task<Consultation> InsertAsync(Consultation consultation)
    {
        _context.Consultations.Add(consultation);
        await _context.SaveChangesAsync();
        return consultation;
    }
    public async Task<bool> CheckAvailabilityAsync(int consultantId, DateTime appointmentTime)
    {
        return !await _context.Consultations
            .AnyAsync(c => c.ConsultantId == consultantId && c.AppointmentTime == appointmentTime);
    }
    public async Task<List<string>> GetUnAvailableSlotsAsync(DateTime date)
    {
        var slots = await _context.Consultations
            .Where(c => c.AppointmentTime.Date == date.Date)
            .Select(c => c.AppointmentTime.ToString("HH:mm"))
            .ToListAsync();
        return slots;
    }
}
