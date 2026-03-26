using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        public int PremiseId { get; set; }
        public Premise? Premise { get; set; }    // Porque?
        public DateTime InspectionDate { get; set; }

        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        public int Score { get; set; }       // (0-100)

        [Required]
        public string Outcome { get; set; } = "";   // (Pass/Fail)
        public string Notes { get; set; } = "";

        public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }
}
