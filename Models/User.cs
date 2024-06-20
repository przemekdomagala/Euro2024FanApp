using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euro2024REST.Models;

public enum UserType
{
    User,
    Admin
}

public class User
{   
    [Key]
    [Required]
    public string Username { get; set; }
    public string ?Password { get; set; } 
    [Column(TypeName="varchar(24)")]
    public UserType Type { get; set; }
}