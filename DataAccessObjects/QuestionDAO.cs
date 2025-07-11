using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class QuestionDAO
    {
        private readonly GenderHealthcareContext _context;

        public QuestionDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestionsByConsultantId(int consultantId)
        {
            return await _context.Questions
                .Where(q => q.ConsultantId == consultantId)
                .Include(q => q.User)
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
        }

        public async Task<Question?> GetQuestionById(int id)
        {
            return await _context.Questions
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.QuestionId == id);
        }

        public async Task UpdateQuestion(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }
    }
}
