using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.Interfaces;

namespace GenderHealthcareServiceManagementSystemPages.Pages.Questions
{
    public class IndexModel : PageModel
    {
        private readonly IQuestionService _iQuestionService;

        public IndexModel(IQuestionService iQuestionService)
        {
            _iQuestionService = iQuestionService;
        }

        public IList<Question> Questions { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Questions = await _iQuestionService.GetAllQuestionsAsync();

            return Page();
        }
    }
}
