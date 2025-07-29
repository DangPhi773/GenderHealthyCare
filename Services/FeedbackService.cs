using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repo;

        public FeedbackService(IFeedbackRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Feedback>> GetAllFeedback(DateTime? from = null, DateTime? to = null) => await _repo.GetAllFeedback(from, to);
        public Task<Feedback?> GetFeedbackById(int id) => _repo.GetFeedbackById(id);
        public Task AddFeedback(Feedback feedback) => _repo.AddFeedback(feedback);
        public Task UpdateFeedback(Feedback feedback) => _repo.UpdateFeedback(feedback);

        public async Task<bool> DeleteFeedback(int id)
        {
            var feedback = await _repo.GetFeedbackById(id);
            if(feedback == null)
            {
                return false;
            }
            feedback.IsDeleted = true;
            await _repo.UpdateFeedback(feedback);
            return true;
        }

        public async Task<(FeedbackStats, List<FeedbackDisplay>)> GetFeedbacksById(int id, string task)
        {
            var feedbacks = await _repo.GetFeedbacksById(id, task);
            return BuildSummary(feedbacks);
        }

        private (FeedbackStats, List<FeedbackDisplay>) BuildSummary(List<Feedback> feedbacks)
        {
            var stats = new FeedbackStats
            {
                AverageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating ?? 0) : 0,
                TotalFeedbacks = feedbacks.Count(),
                RatingCounts = feedbacks
                    .GroupBy(f => f.Rating ?? 0)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            var displays = feedbacks.Select(f => new FeedbackDisplay
            {
                FeedbackId = f.FeedbackId,
                ReviewerName = f.User.Username,
                CreatedAt = f.CreatedAt,
                FeedbackText = f.FeedbackText,
                Rating = f.Rating ?? 0,
                ConsultantName = f.Consultant?.Username,
                ServiceName = f.Service?.Name
            }).ToList();

            return (stats, displays);
        }
        public Task<Feedback?> GetFeedbackByConsultationIdAsync(int userId, int consultationId)
            => _repo.GetFeedbackByConsultationIdAsync(userId, consultationId);

        public Task<Feedback?> GetFeedbackByTestIdAsync(int userId, int testId)
            => _repo.GetFeedbackByTestIdAsync(userId, testId);


        public async Task<(FeedbackStats, List<FeedbackDisplay>)> GetFeedbacksByTask(string task, bool showDeleted)
        {
            var feedbacks = await _repo.GetFeedbacksByTask(task, showDeleted);
            return BuildSummary(feedbacks);
        }

        public async Task<List<Feedback>> GetLatestFeedbackAsync(int count = 5)
        {
            var feedbacks = await _repo.GetLatestFeedbackAsync(count);
            return feedbacks;
        }
    }
}

