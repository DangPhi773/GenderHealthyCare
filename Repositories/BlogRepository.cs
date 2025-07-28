using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;

namespace Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDAO _blogDao;

        public BlogRepository(BlogDAO blogDao)
        {
            _blogDao = blogDao;
        }

        public Task<List<Blog>> GetAllAsync() => _blogDao.GetAllAsync();

        public Task<Blog?> GetByIdAsync(int id) => _blogDao.GetByIdAsync(id);

        public Task AddAsync(Blog blog) => _blogDao.AddAsync(blog);

        public Task UpdateAsync(Blog blog) => _blogDao.UpdateAsync(blog);

        public Task DeleteAsync(int id) => _blogDao.DeleteAsync(id);
    }
}

