using System.ComponentModel.DataAnnotations.Schema;

namespace Euro2024REST.Models;
using System.ComponentModel.DataAnnotations;
    public class Player
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public int Age { get; set; }
        
        public int Height { get; set; }
        public int MarketValue { get; set; }
        [Required]
        [ForeignKey("Team")]
        public string Team { get; set; }
        public string Position { get; set; }
        public string Club { get; set; }
    }