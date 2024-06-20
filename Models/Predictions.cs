using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euro2024REST.Models;

public class Predictions
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Phase { get; set; }
    [Required]
    public string Team { get; set; }
    [Required]
    public int Place { get; set; }
}