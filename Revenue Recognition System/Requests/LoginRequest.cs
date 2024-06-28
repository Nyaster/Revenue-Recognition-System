using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs;

public class LoginRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}