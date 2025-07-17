using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<List<Feedback>> GetAllFeedback();
        Task<Feedback?> GetFeedbackById(int id);
        Task AddFeedback(Feedback feedback);
        Task UpdateFeedback(Feedback feedback);
        Task<(FeedbackStats, List<FeedbackDisplay>)> GetFeedbacksById(int id, string task);
        Task<bool> DeleteFeedback(int id);
        Task<(FeedbackStats, List<FeedbackDisplay>)> GetFeedbacksByTask(string task, bool showDeleted);
    }
}
