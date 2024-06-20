namespace Euro2024REST.Models;
using System.ComponentModel.DataAnnotations;
public class Team
{
    [Key]
    [Required]
    public string Name { get; set; }
    
    public int Rating { get; set; }
    
    
}