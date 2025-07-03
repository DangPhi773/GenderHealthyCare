using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Clinics
{
    public class IndexModel : PageModel
    {
        private readonly GenderHealthcareContext _context;

        public IndexModel(GenderHealthcareContext context)
        {
            _context = context;
        }

        public IList<Clinic> Clinic { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Clinic = await _context.Clinics.ToListAsync();
        }
    }
}
