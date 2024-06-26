namespace Revenue_Recognition_System.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
    public AppUserRole Role { get; set; }
}