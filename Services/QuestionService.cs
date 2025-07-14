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
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _iQuestionRepository;

        public QuestionService(IQuestionRepository iQuestionRepository)
        {
            _iQuestionRepository = iQuestionRepository;
        }

        public async Task<List<Question>> GetAllQuestionsAsync() => await _iQuestionRepository.GetAllQuestionsAsync();
        public async Task<Question?> GetQuestionByIdAsync(int questionId) => await _iQuestionRepository.GetQuestionByIdAsync(questionId);
        public async Task<Question?> AddQuestionAsync(QuestionRequest req, int userId)
        {
            var question = new Question
            {
                UserId = userId,
                QuestionText = $"{req.Subject} - {req.Content}",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
            };

        var createdQuestion = await _iQuestionRepository.AddQuestionAsync(question);
            return createdQuestion;
    }
    public async Task<Question> UpdateQuestionAsync(Question question)
    {
        var updated = await _iQuestionRepository.UpdateQuestionAsync(question);
        return updated;
    }
    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        if (questionId <= 0)
            throw new ArgumentException("Invalid question ID", nameof(questionId));

        var result = await _iQuestionRepository.DeleteQuestionAsync(questionId);
        return result > 0;
    }
    public async Task<List<Question>> GetQuestionsByUserIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID", nameof(userId));

        return await _iQuestionRepository.GetQuestionsByUserIdAsync(userId);
    }
    public async Task<List<Question>> GetQuestionsByConsultantIdAsync(int consultantId)
    {
        if (consultantId <= 0)
            throw new ArgumentException("Invalid consultant ID", nameof(consultantId));

        return await _iQuestionRepository.GetQuestionsByConsultantIdAsync(consultantId);
    }
}
}
