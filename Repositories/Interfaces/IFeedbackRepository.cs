using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetAllFeedback(DateTime? from = null, DateTime? to = null);
        Task<Feedback?> GetFeedbackById(int id);
        Task AddFeedback(Feedback feedback);
        Task UpdateFeedback(Feedback feedback);
        Task<List<Feedback>> GetFeedbacksById(int id, string task);
        Task<Feedback?> GetFeedbackByConsultationIdAsync(int userId, int consultationId);
        Task<Feedback?> GetFeedbackByTestIdAsync(int userId, int testId);
        //Task<bool> DeleteFeedback(int id);
        Task<List<Feedback>> GetFeedbacksByTask(string task, bool showDeleted);
        Task<List<Feedback>> GetLatestFeedbackAsync(int count = 5);
    }
}
