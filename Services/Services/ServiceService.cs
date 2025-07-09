using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ServiceService(IServiceRepository serviceRepository) : IServiceService
    {
        private readonly IServiceRepository _serviceRepository = serviceRepository;

        public Task<List<Service>> GetAllAsync() => _serviceRepository.GetAllAsync();

        public Task<Service?> GetByIdAsync(int id) => _serviceRepository.GetByIdAsync(id);

        public Task<bool> AddAsync(Service service) => _serviceRepository.AddAsync(service);

        public Task<bool> UpdateAsync(Service service) => _serviceRepository.UpdateAsync(service);

        public Task<bool> DeleteAsync(int id) => _serviceRepository.DeleteAsync(id);
    }
}
