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
    //sẽ bỏ đi sau này
    public async Task<List<Consultation>> GetConsultationsByUserIdAsync(int userId)
    {
        return await _context.Consultations
            .Include(c => c.Consultant) 
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.AppointmentTime)
            .ToListAsync();
    }

    //Tìm lịch tư vấn qua Id và Role
    public async Task<List<Consultation>> GetConsultationsByUser(int id, string role)
    {
        try
        {
            var listConsultation
            = _context.Consultations
            .Include(c => c.Consultant)
            .Include(c => c.User)
            .AsQueryable();

            if (role == "Admin" || role == "Manager" || role == "Staff")
                return await listConsultation.ToListAsync();
            if (role == "Customer")
                return await listConsultation
                    .Where(c => c.UserId == id)
                    .ToListAsync();
            if (role == "Consultant")
                return await listConsultation
                    .Where(c => c.ConsultantId == id)
                    .ToListAsync();
            return new List<Consultation>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Tìm lịch tư vấn thông qua userId, userRole và status, Name
    public async Task<List<Consultation>> GetFilteredConsultations(int id, string role, string? status, string? name)
    {
        try
        {
            var listConsultation
            = _context.Consultations
            .Include(c => c.Consultant)
            .Include(c => c.User)
            .AsQueryable();

            listConsultation = role switch
            {
                "Customer" => listConsultation.Where(c => c.UserId == id),
                "Consultant" => listConsultation.Where(c => c.ConsultantId == id),
                _ => listConsultation
            };

            if (!String.IsNullOrEmpty(name))
            {
                listConsultation = role switch
                {
                    "Customer" => listConsultation
                        .Where(c => (c.Consultant.FullName ?? string.Empty).ToLower()
                        .Contains(name.ToLower())),
                    "Consultant" => listConsultation
                        .Where(c => (c.User.FullName ?? string.Empty).ToLower()
                        .Contains(name.ToLower())),
                    _ => listConsultation
                        .Where(c => (c.User.FullName ?? string.Empty).ToLower()
                        .Contains(name.ToLower())
                        || (c.Consultant.FullName ?? string.Empty).ToLower()
                        .Contains(name.ToLower())),
                };
            }

            if (!string.IsNullOrEmpty(status))
            {
                listConsultation = listConsultation
                    .Where(c => c.Status == status);
            }
            return await listConsultation.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Tìm lịch tư vấn thông qua Id
    public async Task<Consultation?> GetConsultationById(int consultationId)
    {
        try
        {
            Consultation? c = await _context.Consultations
                .Include(c => c.Consultant)
                .FirstOrDefaultAsync(c => c.ConsultationId == consultationId);
            return c;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Update lịch tư vấn
    public async Task<bool> UpdateConsultation(Consultation consultation)
    {
        try
        {
            if (consultation != null)
            {
                _context.Entry<Consultation>(consultation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
