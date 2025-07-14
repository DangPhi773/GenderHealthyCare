using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels;

public class QuestionDisplayModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public DateTime SubmissionDate { get; set; }
    public string Status { get; set; } = "Pending"; // e.g., Pending, Answered, Closed
    public string Content { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty; // The expert's response
}
