using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ServiceDAO
    {
        private readonly GenderHealthcareContext _context;

        public ServiceDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }
        public async Task<List<Service>> GetAvailableServicesAsync()
        {
            return await _context.Services
            .Where(s => s.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task<bool> AddAsync(Service service)
        {
            try
            {
                service.CreatedAt = DateTime.Now;
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ServiceDAO][AddAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Service service)
        {
            try
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ServiceDAO][UpdateAsync] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var service = await GetByIdAsync(id);
                if (service != null && service.IsDeleted == false)
                {
                    service.IsDeleted = true;
                    _context.Services.Update(service);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ServiceDAO][DeleteAsync] Error: {ex.Message}");
                return false;
            }
        }
    }
}
