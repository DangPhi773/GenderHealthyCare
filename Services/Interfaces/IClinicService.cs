using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IClinicService
    {
        Task<List<Clinic>> GetAllAsync();
        Task<Clinic?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Clinic clinic);
        Task<bool> UpdateAsync(Clinic clinic);
        Task<bool> DeleteAsync(int clinicId);
    }
}
