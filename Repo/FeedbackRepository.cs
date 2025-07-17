    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;

namespace Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackDAO _dao;

        public FeedbackRepository(FeedbackDAO dao)
        {
            _dao = dao;
        }

        public Task<List<Feedback>> GetAllFeedback() => _dao.GetAllFeedback();
        public Task<Feedback?> GetFeedbackById(int id) => _dao.GetFeedbackById(id);
        public Task AddFeedback(Feedback feedback) => _dao.AddFeedback(feedback);
        public Task UpdateFeedback(Feedback feedback) => _dao.UpdateFeedback(feedback);
        //public Task<bool> DeleteFeedback(int id) => _dao.DeleteFeedback(id);
        public Task<List<Feedback>> GetFeedbacksById(int id, string task) => _dao.GetFeedbacksById(id, task);
        public Task<Feedback?> GetFeedbackByConsultationIdAsync(int userId, int consultationId)
            => _dao.GetFeedbackByConsultationIdAsync(userId, consultationId);

        public Task<Feedback?> GetFeedbackByTestIdAsync(int userId, int testId)
            => _dao.GetFeedbackByTestIdAsync(userId, testId);

        public Task<List<Feedback>> GetFeedbacksByTask(string task, bool showDeleted) => _dao.GetFeedbacksByTask(task, showDeleted);
    }
}
