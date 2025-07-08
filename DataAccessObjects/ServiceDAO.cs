using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class ServiceDAO
    {
        private readonly GenderHealthcareContext _context;

        public ServiceDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetServiceById(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<Service>> GetAllServices()
        {
            return await _context.Services.ToListAsync();
        }
    }
}
