using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public Task<List<Reminder>> GetAllAsync() => _reminderRepository.GetAllAsync();
        public Task<Reminder?> GetByIdAsync(int id) => _reminderRepository.GetByIdAsync(id);
        public Task<List<Reminder>> GetByUserIdAsync(int userId) => _reminderRepository.GetByUserIdAsync(userId);
        public Task<bool> AddAsync(Reminder reminder) => _reminderRepository.AddAsync(reminder);
        public Task<bool> UpdateAsync(Reminder reminder) => _reminderRepository.UpdateAsync(reminder);
        public Task<bool> DeleteAsync(int id) => _reminderRepository.DeleteAsync(id);
    }
}