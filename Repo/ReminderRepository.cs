using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ReminderDAO _dao;

        public ReminderRepository(ReminderDAO dao)
        {
            _dao = dao;
        }

        public Task<List<Reminder>> GetAllAsync() => _dao.GetAllAsync();
        public Task<Reminder?> GetByIdAsync(int id) => _dao.GetByIdAsync(id);
        public Task<List<Reminder>> GetByUserIdAsync(int userId) => _dao.GetByUserIdAsync(userId);
        public Task<bool> AddAsync(Reminder reminder) => _dao.AddAsync(reminder);
        public Task<bool> UpdateAsync(Reminder reminder) => _dao.UpdateAsync(reminder);
        public Task<bool> DeleteAsync(int id) => _dao.DeleteAsync(id);
    }
}