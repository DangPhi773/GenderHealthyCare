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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceDAO _dao;

        public ServiceRepository(ServiceDAO dao)
        {
            _dao = dao;
        }

        public Task<Service?> GetServiceById(int id) => _dao.GetServiceById(id);
        public Task<IEnumerable<Service>> GetAllServices() => _dao.GetAllServices();
    }
}
