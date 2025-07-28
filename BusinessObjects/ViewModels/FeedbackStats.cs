using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ViewModels
{
    public class FeedbackStats
    {
        public double AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
        public Dictionary<int, int> RatingCounts { get; set; } = new();
    }
}
