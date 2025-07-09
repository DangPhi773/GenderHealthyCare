using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ClinicDAO
    {
        private readonly GenderHealthcareContext _context;

        public ClinicDAO(GenderHealthcareContext context)
        {
            _context = context;
        }

        public async Task<List<Clinic>> GetAllClinicsAsync()
        {
            Console.WriteLine("[ClinicDAO][GetAllClinicsAsync] Truy vấn tất cả phòng khám.");
            try
            {
                var clinics = await _context.Clinics.ToListAsync();
                Console.WriteLine($"[ClinicDAO][GetAllClinicsAsync] Số lượng phòng khám tìm thấy: {clinics.Count}");
                return clinics;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClinicDAO][GetAllClinicsAsync] Lỗi khi truy vấn: {ex.Message}");
                return new List<Clinic>();
            }
        }

        public async Task<Clinic?> GetClinicByIdAsync(int id)
        {
            Console.WriteLine($"[ClinicDAO][GetClinicByIdAsync] Truy vấn phòng khám với ClinicId: {id}");
            try
            {
                var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ClinicId == id);
                Console.WriteLine(clinic != null
                    ? $"[ClinicDAO][GetClinicByIdAsync] Tìm thấy phòng khám: {clinic.Name}"
                    : "[ClinicDAO][GetClinicByIdAsync] Không tìm thấy phòng khám.");
                return clinic;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClinicDAO][GetClinicByIdAsync] Lỗi khi truy vấn: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> AddClinicAsync(Clinic clinic)
        {
            Console.WriteLine($"[ClinicDAO][AddClinicAsync] Thêm phòng khám: {clinic.Name}");
            try
            {
                await _context.Clinics.AddAsync(clinic);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[ClinicDAO][AddClinicAsync] Thêm phòng khám thành công, ID: {clinic.ClinicId}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[ClinicDAO][AddClinicAsync] Lỗi cơ sở dữ liệu: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClinicDAO][AddClinicAsync] Lỗi không xác định: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateClinicAsync(Clinic clinic)
        {
            Console.WriteLine($"[ClinicDAO][UpdateClinicAsync] Cập nhật phòng khám ID: {clinic.ClinicId}");
            try
            {
                _context.Clinics.Update(clinic);
                await _context.SaveChangesAsync();
                Console.WriteLine("[ClinicDAO][UpdateClinicAsync] Cập nhật phòng khám thành công.");
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[ClinicDAO][UpdateClinicAsync] Lỗi cơ sở dữ liệu: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClinicDAO][UpdateClinicAsync] Lỗi không xác định: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteClinicAsync(int id)
        {
            Console.WriteLine($"[ClinicDAO][DeleteClinicAsync] Xóa phòng khám ID: {id}");
            try
            {
                var clinic = await _context.Clinics.FindAsync(id);
                if (clinic == null)
                {
                    Console.WriteLine("[ClinicDAO][DeleteClinicAsync] Không tìm thấy phòng khám để xóa.");
                    return false;
                }

                _context.Clinics.Remove(clinic);
                await _context.SaveChangesAsync();
                Console.WriteLine("[ClinicDAO][DeleteClinicAsync] Xóa phòng khám thành công.");
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[ClinicDAO][DeleteClinicAsync] Lỗi cơ sở dữ liệu: {ex.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClinicDAO][DeleteClinicAsync] Lỗi không xác định: {ex.Message}");
                return false;
            }
        }
    }
}
