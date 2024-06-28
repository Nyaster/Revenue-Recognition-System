using System.ComponentModel.DataAnnotations;

namespace Revenue_Recognition_System.DTOs;

public class RegisterRequest
{
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
}