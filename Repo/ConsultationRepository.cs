using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories;
public class ConsultationRepository(ConsultationDAO dao) : IConsultationRepository
{
    private readonly ConsultationDAO _dao = dao;
    public async Task<Consultation> AddAsync(Consultation consultation)
    {
        var created = await _dao.InsertAsync(consultation);
        return created;
    }
    public async Task<bool> IsSlotAvailableAsync(int consultantId, DateTime time)
    {
        return await _dao.CheckAvailabilityAsync(consultantId, time);
    }
    public async Task<List<string>> GetUnavailableSlotsAsync(DateTime date)
    {
        return await _dao.GetUnAvailableSlotsAsync(date);
    }

    public async Task<List<Consultation>> GetConsultationsByUser(int Id, string Role)
    => await _dao.GetConsultationsByUser(Id, Role);

    public async Task<List<Consultation>> GetFilteredConsultations(int userId, string userRole, string? status, string? name)
        => await _dao.GetFilteredConsultations(userId, userRole, status, name);

    public async Task<Consultation?> GetConsultationById(int consultationId)
        => await _dao.GetConsultationById(consultationId);

    public async Task<bool> UpdateConsultation(Consultation consultation)
        => await _dao.UpdateConsultation(consultation);
}
