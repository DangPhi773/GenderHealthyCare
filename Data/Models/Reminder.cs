using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Reminder
{
    public int ReminderId { get; set; }

    public int UserId { get; set; }

    public int? CycleId { get; set; }

    public string ReminderType { get; set; } = null!;

    public DateTime ReminderTime { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual MenstrualCycle? Cycle { get; set; }

    public virtual User User { get; set; } = null!;
}
