using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int UserId { get; set; }

    public int? ConsultantId { get; set; }

    public int? ServiceId { get; set; }

    public int? Rating { get; set; }

    public string? FeedbackText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Consultant { get; set; }

    public virtual Service? Service { get; set; }

    public virtual User User { get; set; } = null!;
}
