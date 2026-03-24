using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        public int PremiseId { get; set; }
        public Premise Premise { get; set; } = null!;   // Porque?
        public DateTime InspectionDate { get; set; }
        public int Score { get; set; }       // (0-100)
        public string Outcome { get; set; }    // (Pass/Fail/Conditional Pass)
        public string Notes { get; set; }

        public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }
}
