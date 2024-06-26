using Revenue_Recognition_System.DTOs;

namespace Revenue_Recognition_System.Services;

public interface IAuthService
{
    public Task<JwtWithRefresh> Login(LoginRequest loginRequest);
    public Task Register(RegisterRequest registerRequest);
    public Task<JwtWithRefresh> Refresh(RefreshTokenRequest refreshRequest);
}