using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class QuestionDAO
    {
        private readonly GenderHealthcareContext _context;

        public QuestionDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            try
            {
                var questions = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Consultant)
                    .ToListAsync();
                return questions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
