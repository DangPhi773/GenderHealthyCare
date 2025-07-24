using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class CreateModel : PageModel
    {
        private readonly IServiceService _serviceService;
        private readonly IHubContext<SignalRServer> _hubContext;

        public CreateModel(IServiceService serviceService, IHubContext<SignalRServer> hubContext)
        {
            _serviceService = serviceService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Service Service { get; set; } = new Service(); // Khởi tạo mặc định để tránh null

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || (role != "Admin" && role != "Staff"))
            {
                return RedirectToPage("/Unauthorized");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || (role != "Admin" && role != "Staff"))
            {
                return RedirectToPage("/Unauthorized");
            }

            if (!ModelState.IsValid || Service == null)
            {
                ModelState.AddModelError(string.Empty, "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.");
                return Page();
            }

            Service.IsDeleted = false;
            Service.CreatedAt = DateTime.UtcNow; // Gán thời gian tạo

            var success = await _serviceService.AddAsync(Service);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Không thể tạo dịch vụ. Vui lòng kiểm tra dữ liệu và thử lại.");
                return Page();
            }

            try
            {
                await _hubContext.Clients.All.SendAsync("ServiceUpdated", Service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Dịch vụ đã được tạo, nhưng không thể thông báo cập nhật.");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}