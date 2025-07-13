using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IClinicRepository
    {
        Task<List<Clinic>> GetAllAsync();
        Task<Clinic?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Clinic clinic);
        Task<bool> UpdateAsync(Clinic clinic);
        Task<bool> DeleteAsync(int clinicId);
        Task<List<Clinic>> GetClinicsByClinicName(string clinicName);
    }
}
