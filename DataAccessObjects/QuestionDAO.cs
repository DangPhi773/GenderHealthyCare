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

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.User)
                    .Include(q => q.Consultant)
                    .FirstOrDefaultAsync(q => q.QuestionId == id);
                return question;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Question> AddQuestionAsync(Question question)
        {
            try
            {
                _context.Questions.Add(question);
                await _context.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            try
            {
                _context.Questions.Update(question);
                await _context.SaveChangesAsync();
                return question;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }   

        public async Task<int> DeleteQuestionAsync(int id)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                {
                    return 0; // Not found
                }

                _context.Questions.Remove(question);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            try
            {
                var questions = await _context.Questions
                    .Where(q => q.UserId == userId)
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
        public async Task<List<Question>> GetQuestionsByConsultantIdAsync(int consultantId)
        {
            try
            {
                var questions = await _context.Questions
                    .Where(q => q.ConsultantId == consultantId)
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
