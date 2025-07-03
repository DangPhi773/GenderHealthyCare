using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly Data.Models.GenderHealthcareContext _context;

        public IndexModel(Data.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        public IList<Service> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Service = await _context.Services.ToListAsync();
        }
    }
}
