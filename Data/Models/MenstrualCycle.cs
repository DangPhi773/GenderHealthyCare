using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class MenstrualCycle
{
    public int CycleId { get; set; }

    public int UserId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly? OvulationDate { get; set; }

    public TimeOnly? PillReminderTime { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual User User { get; set; } = null!;
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
