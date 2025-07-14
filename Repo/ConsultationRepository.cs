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
    public Task<List<Consultation>> GetConsultationsByUserIdAsync(int userId)
    {
        return _dao.GetConsultationsByUserIdAsync(userId);
    }
}
