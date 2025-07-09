using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Role { get; set; } = "Customer"!;

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ConsultantInfo? ConsultantInfo { get; set; }

    public virtual ICollection<Consultation> ConsultationConsultants { get; set; } = new List<Consultation>();

    public virtual ICollection<Consultation> ConsultationUsers { get; set; } = new List<Consultation>();

    public virtual ICollection<Feedback> FeedbackConsultants { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackUsers { get; set; } = new List<Feedback>();

    public virtual ICollection<MenstrualCycle> MenstrualCycles { get; set; } = new List<MenstrualCycle>();

    public virtual ICollection<Question> QuestionConsultants { get; set; } = new List<Question>();

    public virtual ICollection<Question> QuestionUsers { get; set; } = new List<Question>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    public virtual ICollection<UserHistory> UserHistories { get; set; } = new List<UserHistory>();
}
