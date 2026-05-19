using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Ent
{
    public class SessionEnt
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        //Bildförvaring ska ske i Azure Blob Storage
        public string? ImageUrl { get; set; }
        public ICollection<DifficultyLevelEnt> Difficulties { get; set; } = new List<DifficultyLevelEnt>();
    }

}
