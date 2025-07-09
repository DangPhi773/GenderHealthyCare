using BusinessObjects.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IMenstrualCycleRepository
    {
        Task<List<MenstrualCycle>> GetAllAsync();
        Task<MenstrualCycle?> GetByIdAsync(int id);
        Task<List<MenstrualCycle>> GetByUserIdAsync(int userId);
        Task<bool> AddAsync(MenstrualCycle cycle);
        Task<bool> UpdateAsync(MenstrualCycle cycle);
        Task<bool> DeleteAsync(int id);
    }
}
