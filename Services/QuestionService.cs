using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
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
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _iQuestionRepository;

        public QuestionService(IQuestionRepository iQuestionRepository)
        {
            _iQuestionRepository = iQuestionRepository;
        }

        public async Task<List<Question>> GetAllQuestionsAsync() => await _iQuestionRepository.GetAllQuestionsAsync();
        public Task<List<Question>> GetQuestionsByConsultantId(int consultantId)
            => _repository.GetQuestionsByConsultantId(consultantId);

        public Task<Question?> GetQuestionById(int id)
            => _repository.GetQuestionById(id);

        public Task UpdateQuestion(Question question)
            => _repository.UpdateQuestion(question);
    }
}
