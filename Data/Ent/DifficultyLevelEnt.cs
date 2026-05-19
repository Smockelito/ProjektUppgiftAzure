using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Ent
{
    public class DifficultyLevelEnt
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; } = "";
        public ICollection<SessionEnt> Sessions { get; set; } = new List<SessionEnt>();
    }
}
