using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int UserId { get; set; }

    public int? ConsultantId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string? AnswerText { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? AnsweredAt { get; set; }

    public virtual User? Consultant { get; set; }

    public virtual User User { get; set; } = null!;
}
