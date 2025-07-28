using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels;
public class ReportDisplayModel
{
    public int ReportId { get; set; }
    public string ReportTypeName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
