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

        public ClinicRepository(GenderHealthcareContext context)
        {
            _clinicDAO = new ClinicDAO(context);
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
            Console.WriteLine($"[ClinicRepository][UpdateAsync] Cập nhật phòng khám ID: {clinic.ClinicId}");
            return await _clinicDAO.UpdateClinicAsync(clinic);
        }

        public async Task<bool> DeleteAsync(int clinicId)
        {
            Console.WriteLine($"[ClinicRepository][DeleteAsync] Xóa phòng khám với ID: {clinicId}");
            return await _clinicDAO.DeleteClinicAsync(clinicId);
        }
    }
}
