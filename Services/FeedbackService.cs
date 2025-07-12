using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;

        public FeedbackService(IFeedbackRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Feedback>> GetAllFeedback() => _repo.GetAllFeedback();
        public Task<Feedback?> GetFeedbackById(int id) => _repo.GetFeedbackById(id);
        public Task AddFeedback(Feedback feedback) => _repo.AddFeedback(feedback);
        public Task UpdateFeedback(Feedback feedback) => _repo.UpdateFeedback(feedback);
        public Task DeleteFeedback(int id) => _repo.DeleteFeedback(id);
    }
}

