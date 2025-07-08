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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;

        public ServiceService(IServiceRepository repo)
        {
            _repo = repo;
        }

        public Task<Service?> GetServiceById(int id) => _repo.GetServiceById(id);
        public Task<IEnumerable<Service>> GetAllServices() => _repo.GetAllServices();
    }
}
