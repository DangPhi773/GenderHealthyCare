using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int questionId);
        Task<Question?> AddQuestionAsync(QuestionRequest req, int userId);
        Task<Question?> UpdateQuestionAsync(Question question);
        Task<bool> DeleteQuestionAsync(int questionId);
        Task<List<Question>> GetQuestionsByUserIdAsync(int userId);
        Task<List<Question>> GetQuestionsByConsultantIdAsync(int consultantId);
        //Task<List<Question>> GetQuestionsByConsultantId(int consultantId);
        //Task<Question?> GetQuestionById(int id);
        //Task UpdateQuestion(Question question);
    }
}
