using Microsoft.AspNetCore.Mvc;

namespace Revenue_Recognition_System.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    public IActionResult Login(LoginRequest loginRequest)
}