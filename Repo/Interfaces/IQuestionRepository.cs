using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<int> AddQuestionAsync(Question question);
        Task<int> UpdateQuestionAsync(Question question);
        Task<int> DeleteQuestionAsync(int id);
        Task<List<Question>> GetQuestionsByUserIdAsync(int userId);
        Task<List<Question>> GetQuestionsByConsultantIdAsync(int consultantId);
        Task<List<Question>> GetQuestionsByConsultantId(int consultantId);
        Task<Question?> GetQuestionById(int id);
        Task UpdateQuestion(Question question);
    }
}


