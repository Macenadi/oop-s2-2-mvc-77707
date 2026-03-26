using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    public class FollowUp
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public Inspection? Inspection { get; set; } 

        public DateTime DueDate { get; set; }

        [Required]
        public string Status { get; set; } = "";    //(Open/Closed)
        public DateTime? ClosedDate { get; set; }
    }
}
