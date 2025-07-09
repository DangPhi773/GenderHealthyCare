using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceDAO _serviceDAO;

        public ServiceRepository(GenderHealthcareContext context)
        {
            _serviceDAO = new ServiceDAO(context);
        }

        public Task<List<Service>> GetAllAsync() => _serviceDAO.GetAllAsync();

        public Task<Service?> GetByIdAsync(int id) => _serviceDAO.GetByIdAsync(id);

        public Task<bool> AddAsync(Service service) => _serviceDAO.AddAsync(service);

        public Task<bool> UpdateAsync(Service service) => _serviceDAO.UpdateAsync(service);

        public Task<bool> DeleteAsync(int id) => _serviceDAO.DeleteAsync(id);
    }
}
