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
        Task<List<Feedback>> GetAllFeedback();
        Task<Feedback?> GetFeedbackById(int id);
        Task AddFeedback(Feedback feedback);
        Task UpdateFeedback(Feedback feedback);
        Task<List<Feedback>> GetFeedbacksById(int id, string task);
        //Task<bool> DeleteFeedback(int id);
        Task<List<Feedback>> GetFeedbacksByTask(string task, bool showDeleted);
    }
}
