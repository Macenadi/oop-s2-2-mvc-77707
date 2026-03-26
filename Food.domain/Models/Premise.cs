using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food.domain.Models
{
    public class Premise
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Town { get; set; } = "";

        [Required]
        public string RiskRating { get; set; } = "";   //(Low/Medium/High)

        public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
