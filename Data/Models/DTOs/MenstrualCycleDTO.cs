using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTOs
{
    public class MenstrualCycleDTO
    {
        public int CycleId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? OvulationDate { get; set; }
        public TimeOnly? PillReminderTime { get; set; }
        public string? Notes { get; set; }

        public int CycleLength
        {
            get
            {
                if (StartDate != null && EndDate != null)
                {
                    return (EndDate.Value.ToDateTime(TimeOnly.MinValue) - StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days + 1;
                }
                return 0;
            }
        }
    }
}
