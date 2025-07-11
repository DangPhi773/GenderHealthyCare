using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question>> GetQuestionsByConsultantId(int consultantId);
        Task<Question?> GetQuestionById(int id);
        Task UpdateQuestion(Question question);
    }
}
