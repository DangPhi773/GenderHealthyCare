using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects.Models;
using Services.Interfaces;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GenderHealthcareServiceManagementSystemPages.Pages.ServiceManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IServiceService _serviceService;
        private readonly IHubContext<SignalRServer> _hubContext;


        public DeleteModel(IServiceService serviceService, IHubContext<SignalRServer> hubContext)
        {
            _serviceService = serviceService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin" && role != "Staff")
            {
                return RedirectToPage("/Unauthorized");
            }
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetByIdAsync(id.Value);


            Service = service;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetByIdAsync(id.Value);


            var result = await _serviceService.DeleteAsync(service.ServiceId);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Xóa dịch vụ thất bại.");
                return Page();
            }
            await _hubContext.Clients.All.SendAsync("ServiceUpdated", service);

            return RedirectToPage("./Index");
        }
    }
}
