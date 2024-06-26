using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Services;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var jwtWithRefresh = await _authService.Login(loginRequest);
        return Ok(jwtWithRefresh);
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        await _authService.Register(registerRequest);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest refreshToken)
    {
        var jwtWithRefresh = await _authService.Refresh(refreshToken);
        return Ok(jwtWithRefresh);
    }
}