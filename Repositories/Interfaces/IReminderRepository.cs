using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IReminderRepository
    {
        Task<List<Reminder>> GetAllAsync();
        Task<Reminder?> GetByIdAsync(int id);
        Task<List<Reminder>> GetByUserIdAsync(int userId);
        Task<bool> AddAsync(Reminder reminder);
        Task<bool> UpdateAsync(Reminder reminder);
        Task<bool> DeleteAsync(int id);
        Task<List<Reminder>> GetDueRemindersAsync(DateTime currentTime);
    }
}