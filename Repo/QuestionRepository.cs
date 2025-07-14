using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuestionDAO _questionDAO;

        public QuestionRepository(QuestionDAO questionDAO)
        {
            _questionDAO = questionDAO;
        }
        public async Task<List<Question>> GetAllQuestionsAsync() => await _questionDAO.GetAllQuestionsAsync();
        public async Task<Question?> GetQuestionByIdAsync(int id) => await _questionDAO.GetQuestionByIdAsync(id);
        public async  Task<Question> AddQuestionAsync(Question question) => await _questionDAO.AddQuestionAsync(question);
        public async  Task<Question> UpdateQuestionAsync(Question question)
        {
            try
            {
                var existingQuestion = await _questionDAO.GetQuestionByIdAsync(question.QuestionId);
                return existingQuestion == null
                    ? throw new KeyNotFoundException($"Question with ID {question.QuestionId} not found")
                    : await _questionDAO.UpdateQuestionAsync(question);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating question: {ex.Message}");
            }
        }
        public async Task<int> DeleteQuestionAsync(int id) => await _questionDAO.DeleteQuestionAsync(id);
        public async Task<List<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            try
            {
                return await _questionDAO.GetQuestionsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving questions for user {userId}: {ex.Message}");
            }
        }
        public async Task<List<Question>> GetQuestionsByConsultantIdAsync(int consultantId)
        {
            try
            {
                return await _questionDAO.GetQuestionsByConsultantIdAsync(consultantId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving questions for consultant {consultantId}: {ex.Message}");
            }
        }

        public Task<List<Question>> GetQuestionsByConsultantId(int consultantId)
            => _questionDAO.GetQuestionsByConsultantId(consultantId);

        public Task<Question?> GetQuestionById(int id)
            => _questionDAO.GetQuestionById(id);

        public Task UpdateQuestion(Question question)
            => _questionDAO.UpdateQuestion(question);
    }
}
