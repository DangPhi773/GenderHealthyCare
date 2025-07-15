using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Admin.UserManager
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.Models.GenderHealthcareContext _context;

        public IndexModel(BusinessObjects.Models.GenderHealthcareContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
