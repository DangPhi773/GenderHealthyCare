using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _clinicRepository;

        public ClinicService(IClinicRepository clinicRepository)
        {
            _clinicRepository = clinicRepository;
        }

        public async Task<List<Clinic>> GetAllAsync()
        {
            Console.WriteLine("[ClinicService][GetAllAsync] Gọi lấy danh sách tất cả phòng khám.");
            return await _clinicRepository.GetAllAsync();
        }

        public async Task<Clinic?> GetByIdAsync(int id)
        {
            Console.WriteLine($"[ClinicService][GetByIdAsync] Gọi lấy phòng khám với ID: {id}");
            return await _clinicRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Clinic clinic)
        {
            Console.WriteLine($"[ClinicService][CreateAsync] Gọi tạo mới phòng khám: {clinic.Name}");
            return await _clinicRepository.CreateAsync(clinic);
        }

        public async Task<bool> UpdateAsync(Clinic clinic)
        {
            Console.WriteLine($"[ClinicService][UpdateAsync] Gọi cập nhật phòng khám ID: {clinic.ClinicId}");
            return await _clinicRepository.UpdateAsync(clinic);
        }

        public async Task<bool> DeleteAsync(int clinicId)
        {
            Console.WriteLine($"[ClinicService][DeleteAsync] Gọi xóa phòng khám với ID: {clinicId}");
            return await _clinicRepository.DeleteAsync(clinicId);
        }

        public async Task<List<Clinic>> GetClinicsByClinicName(string clinicName) => await _clinicRepository.GetClinicsByClinicName(clinicName);
    }
}
