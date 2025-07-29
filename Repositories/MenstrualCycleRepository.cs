using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;

public class MenstrualCycleRepository : IMenstrualCycleRepository
{
    private readonly MenstrualCycleDAO _dao;

    public MenstrualCycleRepository(MenstrualCycleDAO dao)
    {
        _dao = dao;
    }

    public Task<List<MenstrualCycle>> GetAllAsync() => _dao.GetAllAsync();
    public Task<MenstrualCycle?> GetByIdAsync(int id) => _dao.GetByIdAsync(id);
    public Task<List<MenstrualCycle>> GetByUserIdAsync(int userId) => _dao.GetByUserIdAsync(userId);
    public Task<bool> AddAsync(MenstrualCycle cycle) => _dao.AddAsync(cycle);
    public Task<bool> UpdateAsync(MenstrualCycle cycle) => _dao.UpdateAsync(cycle);
    public Task<bool> DeleteAsync(int id) => _dao.DeleteAsync(id);
    public Task<double> GetAverageCycleLengthAsync(int userId) => _dao.GetAverageCycleLengthAsync(userId);
    public Task<DateOnly?> PredictNextCycleStartAsync(int userId) => _dao.PredictNextCycleStartAsync(userId);
    public Task<DateOnly?> PredictOvulationDateAsync(int userId, DateOnly cycleStartDate) => _dao.PredictOvulationDateAsync(userId, cycleStartDate);
    public Task<DateOnly?> PredictCycleEndDateAsync(int userId, DateOnly cycleStartDate) => _dao.PredictCycleEndDateAsync(userId, cycleStartDate);
    public Task<string> AnalyzeCurrentCycleAsync(int userId) => _dao.AnalyzeCurrentCycleAsync(userId);
}