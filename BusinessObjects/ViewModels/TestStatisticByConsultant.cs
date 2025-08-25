using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels
{
    public class TestStatisticByConsultant
    {
        public string ConsultantName { get; set; } = string.Empty;
        public int TestCount { get; set; }
        public DateTime Date { get; set; }
    }
}
