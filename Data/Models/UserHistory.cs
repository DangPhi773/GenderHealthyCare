using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class UserHistory
{
    public int HistoryId { get; set; }

    public int UserId { get; set; }

    public string ActionType { get; set; } = null!;

    public int ActionId { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual User User { get; set; } = null!;
}
