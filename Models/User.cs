using System.ComponentModel.DataAnnotations;

namespace GamePortal.Models;

public class User
{
    [Required]
    public string Name { get; set; }
}

