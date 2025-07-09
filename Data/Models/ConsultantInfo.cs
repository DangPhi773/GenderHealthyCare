using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ConsultantInfo
{
    public int ConsultantId { get; set; }

    public string? Qualifications { get; set; }

    public int? ExperienceYears { get; set; }

    public string? Specialization { get; set; }

    public byte[]? ProfileImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Consultant { get; set; } = null!;
}
