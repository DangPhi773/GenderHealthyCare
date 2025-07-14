using BusinessObjects.Models;
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
    }
}
