using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class BlogDAO
    {
        private readonly GenderHealthcareContext _context;

        public BlogDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllAsync() =>
            await _context.Blogs.Include(b => b.Author).OrderByDescending(b => b.CreatedAt).ToListAsync();

        public async Task<Blog?> GetByIdAsync(int id) =>
            await _context.Blogs.Include(b => b.Author).FirstOrDefaultAsync(b => b.BlogId == id);

        public async Task AddAsync(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
