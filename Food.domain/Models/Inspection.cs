using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    internal class Inspection
    {
        public int Id { get; set; }
        public int PremiseId { get; set; }
        public DateTime InspectionDate { get; set; }

        public string Score { get; set; }       // (0-100)
        public string Outcome { get; set; }    // (Pass/Fail/Conditional Pass)
        public string Notes { get; set; }
    }
}
