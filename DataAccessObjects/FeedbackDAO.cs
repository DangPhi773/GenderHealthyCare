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

        public async Task<List<Feedback>> GetFeedbacksById(int id, string task)
        {
            try
            {
                var query = _context.Feedbacks
                    .Include(f => f.User)
                    .AsQueryable();
                if (task == "service")
                    return await query
                        .Where(q => q.ServiceId == id)
                        .ToListAsync();
                else if (task == "consultant")
                    return await query
                        .Where(q => q.ConsultantId == id)
                        .ToListAsync();
                return new List<Feedback>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Feedback>> GetFeedbacksByTask(string task, bool showDeleted)
        {
            try
            {
                var query = _context.Feedbacks
                    .Include(f => f.User)
                    .AsQueryable();
                if (task == "service")
                    query = query
                      .Where(q => q.ServiceId.HasValue);
                else if (task == "consultant")
                    query = query
                       .Where(q => q.ConsultantId.HasValue);
                if (showDeleted)
                {
                    return await query.Where(q => q.IsDeleted == true).ToListAsync();
                }
                else
                {
                    return await query.Where(q => q.IsDeleted == false).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        //public async Task<bool> DeleteFeedback(int id)
        //{
        //    try
        //    {
        //        var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
        //        if (feedback != null)
        //        {
        //            _context.Feedbacks.Remove(feedback);
        //            await _context.SaveChangesAsync();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<Feedback?> GetFeedbackByConsultationIdAsync(int userId, int consultationId)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f =>
                    f.UserId == userId &&
                    f.ConsultationId == consultationId &&
                    (f.IsDeleted == null || f.IsDeleted == false));
        }

        public async Task<Feedback?> GetFeedbackByTestIdAsync(int userId, int testId)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f =>
                    f.UserId == userId &&
                    f.TestId == testId &&
                    (f.IsDeleted == null || f.IsDeleted == false));
        }
        public async Task<List<Feedback>> GetLatestFeedbackAsync(int count)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Consultant)
                .Include(f => f.Service)
                .OrderByDescending(f => f.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
