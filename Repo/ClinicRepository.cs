using BusinessObjects.Models;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly ClinicDAO _clinicDAO;

        public ClinicRepository(ClinicDAO clinicDAO)
        {
            _clinicDAO = clinicDAO;
        }

        public async Task<List<Clinic>> GetAllAsync()
        {
            Console.WriteLine("[ClinicRepository][GetAllAsync] Lấy danh sách tất cả phòng khám.");
            return await _clinicDAO.GetAllClinicsAsync();
        }

        public async Task<Clinic?> GetByIdAsync(int id)
        {
            Console.WriteLine($"[ClinicRepository][GetByIdAsync] Lấy phòng khám với ID: {id}");
            return await _clinicDAO.GetClinicByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Clinic clinic)
        {
            Console.WriteLine($"[ClinicRepository][CreateAsync] Tạo mới phòng khám: {clinic.Name}");
            clinic.CreatedAt = DateTime.Now;
            return await _clinicDAO.AddClinicAsync(clinic);
        }

        public async Task<bool> UpdateAsync(Clinic clinic)
        {
            
            return await _clinicDAO.UpdateClinicAsync(clinic);
        }

        public async Task<List<Clinic>> GetClinicsByClinicName(string clinicName, bool showDeleted) => await _clinicDAO.GetClinicsByClinicName(clinicName, showDeleted);

        public async Task<List<Clinic>> GetAllAsync(bool showDeleted) => await _clinicDAO.GetAllClinicsAsync(showDeleted);
    }
}
