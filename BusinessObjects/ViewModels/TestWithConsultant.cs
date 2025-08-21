using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels
{
    public class TestWithConsultant
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
