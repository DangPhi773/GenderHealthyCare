using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenderHealthcareServiceManagementSystemPages.Pages
{
    public class MenstrualCycleModel : PageModel
    {
        private readonly IMenstrualCycleService _cycleService;

        public MenstrualCycleModel(IMenstrualCycleService cycleService)
        {
            _cycleService = cycleService;
        }

        [BindProperty]
        public MenstrualCycle CycleModel { get; set; } = new();

        public List<MenstrualCycle> MenstrualCycles { get; set; } = new();

        private int CurrentUserId => 1;

        public async Task<IActionResult> OnGetAsync()
        {
            MenstrualCycles = await _cycleService.GetByUserIdAsync(CurrentUserId);
            return Page();
        }

        public async Task<IActionResult> OnPostSaveCycleAsync()
        {
            if (!ModelState.IsValid)
            {
                MenstrualCycles = await _cycleService.GetByUserIdAsync(CurrentUserId);
                return Page(); 
            }

            CycleModel.UserId = CurrentUserId;
            try
            {
                if (CycleModel.CycleId > 0)
                {
                    await _cycleService.UpdateAsync(CycleModel);
                }
                else
                {
                    await _cycleService.AddAsync(CycleModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi lưu chu kỳ: " + ex.Message);
                MenstrualCycles = await _cycleService.GetByUserIdAsync(CurrentUserId);
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            await _cycleService.DeleteAsync(id);
            return RedirectToPage();
        }

        public async Task<JsonResult> OnGetGetCycleAsync(int id)
        {
            var cycle = await _cycleService.GetByIdAsync(id);
            if (cycle == null || cycle.UserId != CurrentUserId)
            {
                return new JsonResult(new { success = false, message = "Không tìm thấy chu kỳ" });
            }

            return new JsonResult(new { success = true, data = cycle });
        }
    }
}
