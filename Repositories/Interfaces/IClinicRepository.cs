using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IClinicRepository
    {
        Task<List<Clinic>> GetAllAsync();
        Task<List<Clinic>> GetAllAsync(bool showDeleted);
        Task<Clinic?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Clinic clinic);
        Task<bool> UpdateAsync(Clinic clinic);
        //Task<bool> DeleteAsync(int clinicId);
        Task<List<Clinic>> GetClinicsByClinicName(string clinicName, bool showDeleted);
        Task<bool> UploadImageAsync(int clinicId, byte[] imageData);
    }
}
