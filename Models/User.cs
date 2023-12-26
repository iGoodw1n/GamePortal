using System.ComponentModel.DataAnnotations;

namespace GamePortal.Models;

public class User
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = null!;
}

