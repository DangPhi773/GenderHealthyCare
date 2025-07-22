using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Thêm namespace này cho IFormFile
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO; // Thêm namespace này cho MemoryStream

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class DetailsModel : PageModel
    {
        private readonly IClinicService _iClinicService;

        public DetailsModel(IClinicService iClinicService)
        {
            _iClinicService = iClinicService;
        }

        [BindProperty]
        public Clinic Clinic { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                return RedirectToPage("/Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var clinic = await _iClinicService.GetByIdAsync((int)id);
            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                Clinic = clinic;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateClinicAsync(string action)
        {
            if (action == "update")
            {
                if (!ModelState.IsValid)
                {
                    // Lấy lại dữ liệu Clinic nếu ModelState không hợp lệ để hiển thị lại trên form
                    var currentClinic = await _iClinicService.GetByIdAsync(Clinic.ClinicId);
                    if (currentClinic != null)
                    {
                        Clinic.Image = currentClinic.Image; // Giữ lại ảnh hiện có nếu không có ảnh mới được tải lên
                    }
                    return Page();
                }

                var success = await _iClinicService.UpdateAsync(Clinic);

                if (!success)
                {
                    TempData["ErrorMessage"] = "Cập nhật thông tin phòng khám thất bại.";
                    // Lấy lại dữ liệu Clinic để hiển thị lại trên form nếu cập nhật thất bại
                    var currentClinic = await _iClinicService.GetByIdAsync(Clinic.ClinicId);
                    if (currentClinic != null)
                    {
                        Clinic.Image = currentClinic.Image;
                    }
                    return Page();
                }
                TempData["SuccessMessage"] = "Cập nhật thông tin phòng khám thành công!";
                return RedirectToPage(new { id = Clinic.ClinicId });
            }

            if (action == "delete")
            {
                var deleted = await _iClinicService.DeleteAsync(Clinic.ClinicId);
                if (!deleted)
                {
                    TempData["ErrorMessage"] = "Xóa phòng khám thất bại.";
                    return NotFound();
                }
                TempData["SuccessMessage"] = "Xóa phòng khám thành công!";
                return RedirectToPage("Index");
            }
            return Page();
        }

  
        public async Task<IActionResult> OnPostUploadImageAsync(int clinicId)
        {
            if (ImageUpload == null || ImageUpload.Length == 0)
            {
                return new JsonResult(new { success = false, message = "Không có tệp ảnh nào được chọn." });
            }

            using (var memoryStream = new MemoryStream())
            {
                await ImageUpload.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var success = await _iClinicService.UploadImageAsync(clinicId, imageData);

                if (success)
                {
                    return new JsonResult(new { success = true, message = "Ảnh đã được tải lên thành công." });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Không thể tải ảnh lên." });
                }
            }
        }
    }
}