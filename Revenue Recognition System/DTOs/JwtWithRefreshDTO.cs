namespace Revenue_Recognition_System.DTOs;

public class JwtWithRefreshDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}