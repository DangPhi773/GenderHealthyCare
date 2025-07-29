using BusinessObjects.Models;
using System;
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
        Task<double> GetAverageCycleLengthAsync(int userId);
        Task<DateOnly?> PredictNextCycleStartAsync(int userId);
        Task<DateOnly?> PredictOvulationDateAsync(int userId, DateOnly cycleStartDate);
        Task<DateOnly?> PredictCycleEndDateAsync(int userId, DateOnly cycleStartDate);
        Task<string> AnalyzeCurrentCycleAsync(int userId);


    }
}