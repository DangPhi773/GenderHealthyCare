using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class MenstrualCycleRepository : IMenstrualCycleRepository
    {
        private readonly MenstrualCycleDAO _cycleDAO;

        public MenstrualCycleRepository(GenderHealthcareContext context)
        {
            _cycleDAO = new MenstrualCycleDAO(context);
        }

        public Task<List<MenstrualCycle>> GetAllAsync() => _cycleDAO.GetAllAsync();

        public Task<MenstrualCycle?> GetByIdAsync(int id) => _cycleDAO.GetByIdAsync(id);

        public Task<List<MenstrualCycle>> GetByUserIdAsync(int userId) => _cycleDAO.GetByUserIdAsync(userId);

        public Task<bool> AddAsync(MenstrualCycle cycle) => _cycleDAO.AddAsync(cycle);

        public Task<bool> UpdateAsync(MenstrualCycle cycle) => _cycleDAO.UpdateAsync(cycle);

        public Task<bool> DeleteAsync(int id) => _cycleDAO.DeleteAsync(id);
    }
}
