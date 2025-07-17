using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels
{
    public class FeedbackDisplay
    {
        public int FeedbackId { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; }
        public string? FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ConsultantName { get; set; }
        public string? ServiceName {  get; set; }
    }
}
