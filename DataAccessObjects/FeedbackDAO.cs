using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class FeedbackDAO
    {
        private readonly GenderHealthcareContext _context;

        public FeedbackDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> GetAllFeedback()
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Consultant)
                .Include(f => f.Service)
                .ToListAsync();
        }

        public async Task<Feedback?> GetFeedbackById(int id)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Consultant)
                .Include(f => f.Service)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);
        }

        public async Task AddFeedback(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedback(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }
    }
}
