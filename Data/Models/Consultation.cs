using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Consultation
{
    public int ConsultationId { get; set; }

    public int UserId { get; set; }

    public int ConsultantId { get; set; }

    public DateTime AppointmentTime { get; set; }

    public string? Status { get; set; }

    public string? MeetingLink { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User Consultant { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User User { get; set; } = null!;
}
