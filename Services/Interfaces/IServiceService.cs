using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        Task<List<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<bool> AddAsync(Service service);
        Task<bool> UpdateAsync(Service service);
        Task<bool> DeleteAsync(int id);
    }
}
