using System.ComponentModel.DataAnnotations.Schema;

namespace Euro2024REST.Models;
using System.ComponentModel.DataAnnotations;
public class Match
{
    [Key]
    public int Id { get; set; }

    [Required] 
    [ForeignKey("Team")]
    public string Team1 { get; set; }

    [Required] 
    [ForeignKey("Team")]
    public string Team2 { get; set; }

    public int Score1 { get; set; }
    public int Score2 { get; set; }

    public DateTime Date { get; set; }
    public string Venue { get; set; }
    public string Phase { get; set; }
}