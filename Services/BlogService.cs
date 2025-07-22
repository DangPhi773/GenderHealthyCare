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
        public class BlogService : IBlogService
        {
            private readonly IBlogRepository _blogRepository;

            public BlogService(IBlogRepository blogRepository)
            {
                _blogRepository = blogRepository;
            }

            public Task<List<Blog>> GetAllAsync() => _blogRepository.GetAllAsync();

            public Task<Blog?> GetByIdAsync(int id) => _blogRepository.GetByIdAsync(id);

            public Task AddAsync(Blog blog) => _blogRepository.AddAsync(blog);

            public Task UpdateAsync(Blog blog) => _blogRepository.UpdateAsync(blog);

            public Task DeleteAsync(int id) => _blogRepository.DeleteAsync(id);
        }
    }

