using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces;
public interface IConsultationRepository
{
    Task<Consultation> AddAsync(Consultation consultation);
    Task<bool> IsSlotAvailableAsync(int consultantId, DateTime appointmentTime);
    Task<List<string>> GetUnavailableSlotsAsync(DateTime date);
}
