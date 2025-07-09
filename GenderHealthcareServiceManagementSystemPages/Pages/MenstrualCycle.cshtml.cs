using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using BusinessObjects.Models.DTOs;

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

        public List<MenstrualCycleDTO> CycleDTOs { get; set; } = new();

        public DateOnly? PredictedNextCycle { get; set; }

        public DateOnly? PredictedCycleEnd { get; set; }

        public string CurrentCycleStatus { get; set; }

        private int CurrentUserId
        {
            get
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                {
                    throw new UnauthorizedAccessException("Phiên đăng nhập đã hết hạn hoặc ID người dùng không hợp lệ.");
                }
                return userId;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var menstrualCycles = await _cycleService.GetByUserIdAsync(CurrentUserId);
                CycleDTOs = menstrualCycles.Select(c => new MenstrualCycleDTO
                {
                    CycleId = c.CycleId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    OvulationDate = c.OvulationDate,
                    PillReminderTime = c.PillReminderTime,
                    Notes = c.Notes
                }).ToList();

                PredictedNextCycle = await _cycleService.PredictNextCycleStartAsync(CurrentUserId);

                var currentCycle = CycleDTOs.FirstOrDefault(c => c.EndDate == null || c.EndDate.Value.ToDateTime(TimeOnly.MinValue) >= DateTime.Today);
                if (currentCycle != null && currentCycle.StartDate != null && currentCycle.EndDate == null)
                {
                    PredictedCycleEnd = await _cycleService.PredictCycleEndDateAsync(CurrentUserId, currentCycle.StartDate.Value);
                }

                CurrentCycleStatus = await _cycleService.AnalyzeCurrentCycleAsync(CurrentUserId);

                return Page();
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để tiếp tục.";
                return RedirectToPage("/Account/Login");
            }
        }

        public async Task<IActionResult> OnPostSaveCycleAsync()
        {
            Console.WriteLine("OnPostSaveCycleAsync called");
            Console.WriteLine($"Received CycleModel: CycleId={CycleModel.CycleId}, StartDate={CycleModel.StartDate}, EndDate={CycleModel.EndDate}, OvulationDate={CycleModel.OvulationDate}, PillReminderTime={CycleModel.PillReminderTime}, Notes={CycleModel.Notes}, UserId={CycleModel.UserId}");

            if (CycleModel.EndDate != null && CycleModel.EndDate < CycleModel.StartDate)
            {
                Console.WriteLine("Validation failed: EndDate < StartDate");
                return new JsonResult(new { success = false, message = "Ngày kết thúc không được nhỏ hơn ngày bắt đầu." });
            }

            if (CycleModel.OvulationDate.HasValue && (CycleModel.OvulationDate < CycleModel.StartDate || (CycleModel.EndDate.HasValue && CycleModel.OvulationDate > CycleModel.EndDate)))
            {
                Console.WriteLine("Validation failed: OvulationDate out of range");
                return new JsonResult(new { success = false, message = "Ngày rụng trứng cần nằm trong khoảng từ ngày bắt đầu đến ngày kết thúc." });
            }

            if (CycleModel.StartDate != null && CycleModel.OvulationDate == null)
            {
                CycleModel.OvulationDate = await _cycleService.PredictOvulationDateAsync(CurrentUserId, CycleModel.StartDate.Value);
                Console.WriteLine($"Predicted OvulationDate: {CycleModel.OvulationDate}");
            }

            CycleModel.UserId = CurrentUserId;
            Console.WriteLine($"Assigned UserId: {CycleModel.UserId}");
            ModelState.Remove("CycleModel.User");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine($"ModelState invalid: {string.Join(", ", errors)}");
                return new JsonResult(new { success = false, message = "Vui lòng điền đầy đủ các trường bắt buộc." });
            }

            try
            {
                if (CycleModel.CycleId > 0)
                {
                    var existingCycle = await _cycleService.GetByIdAsync(CycleModel.CycleId);
                    Console.WriteLine($"ExistingCycle: {existingCycle?.CycleId}, ExistingUserId={existingCycle?.UserId}, CurrentUserId={CurrentUserId}");
                    if (existingCycle == null || existingCycle.UserId != CurrentUserId)
                    {
                        Console.WriteLine("Validation failed: ExistingCycle null or UserId mismatch");
                        return new JsonResult(new { success = false, message = "Không thể cập nhật chu kỳ. Vui lòng kiểm tra lại." });
                    }
                    await _cycleService.UpdateAsync(CycleModel);
                    Console.WriteLine("Update completed successfully");
                }
                else
                {
                    await _cycleService.AddAsync(CycleModel);
                }
                return new JsonResult(new { success = true, message = "Chu kỳ đã được cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return new JsonResult(new { success = false, message = "Lỗi khi lưu chu kỳ: " + ex.Message });
            }
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            try
            {
                var cycle = await _cycleService.GetByIdAsync(id);
                if (cycle == null || cycle.UserId != CurrentUserId)
                {
                    TempData["ErrorMessage"] = "Không thể xóa chu kỳ. Vui lòng kiểm tra lại.";
                }
                else
                {
                    await _cycleService.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Chu kỳ đã được xóa thành công!";
                }
                return RedirectToPage();
            }
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để tiếp tục.";
                return RedirectToPage("/Account/Login");
            }
        }

        public async Task<JsonResult> OnGetGetCycleAsync(int id)
        {
            try
            {
                var cycle = await _cycleService.GetByIdAsync(id);
                if (cycle == null || cycle.UserId != CurrentUserId)
                {
                    return new JsonResult(new { success = false, message = "Không tìm thấy chu kỳ hoặc bạn không có quyền truy cập." });
                }

                var dto = new MenstrualCycleDTO
                {
                    CycleId = cycle.CycleId,
                    StartDate = cycle.StartDate,
                    EndDate = cycle.EndDate,
                    OvulationDate = cycle.OvulationDate,
                    PillReminderTime = cycle.PillReminderTime,
                    Notes = cycle.Notes
                };

                return new JsonResult(new { success = true, data = dto });
            }
            catch (UnauthorizedAccessException)
            {
                return new JsonResult(new { success = false, message = "Vui lòng đăng nhập để tiếp tục." });
            }
        }
    }
}