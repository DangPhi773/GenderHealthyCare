using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Test
{
    public int TestId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public DateTime AppointmentTime { get; set; }

    public string? Status { get; set; }

    public string? Result { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
