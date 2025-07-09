using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MenstrualCycleService : IMenstrualCycleService
    {
        private readonly IMenstrualCycleRepository _cycleRepository;

        public MenstrualCycleService(IMenstrualCycleRepository cycleRepository)
        {
            _cycleRepository = cycleRepository;
        }

        public Task<List<MenstrualCycle>> GetAllAsync() => _cycleRepository.GetAllAsync();

        public Task<MenstrualCycle?> GetByIdAsync(int id) => _cycleRepository.GetByIdAsync(id);

        public Task<List<MenstrualCycle>> GetByUserIdAsync(int userId) => _cycleRepository.GetByUserIdAsync(userId);

        public Task<bool> AddAsync(MenstrualCycle cycle) => _cycleRepository.AddAsync(cycle);

        public Task<bool> UpdateAsync(MenstrualCycle cycle) => _cycleRepository.UpdateAsync(cycle);

        public Task<bool> DeleteAsync(int id) => _cycleRepository.DeleteAsync(id);
    }
}
