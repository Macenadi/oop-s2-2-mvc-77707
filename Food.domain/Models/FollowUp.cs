using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    public class FollowUp
    {
        public int Id { get; set; }
        public int InspectionId { get; set; } 
        public DateTime DueDate { get; set; }
        public string Status { get; set; }    // (Open/Closed)
        public DateTime? ClosedDate { get; set; }
    }
}
