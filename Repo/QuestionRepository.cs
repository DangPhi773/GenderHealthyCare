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
        private readonly QuestionDAO _dao;

        public QuestionRepository(QuestionDAO dao)
        {
            _dao = dao;
        }

        public Task<List<Question>> GetQuestionsByConsultantId(int consultantId)
            => _dao.GetQuestionsByConsultantId(consultantId);

        public Task<Question?> GetQuestionById(int id)
            => _dao.GetQuestionById(id);

        public Task UpdateQuestion(Question question)
            => _dao.UpdateQuestion(question);
    }
}

