using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Revenue_Recognition_System.DTOs;
using Revenue_Recognition_System.Helpers;
using Revenue_Recognition_System.Models;
using Revenue_Recognition_System.Repositories;

namespace Revenue_Recognition_System.Services;

public class AuthService : IAuthService
{
    private IUsersBaseRepository _baseRepository;
    private IConfiguration _configuration;

    public AuthService(IUsersBaseRepository baseRepository, IConfiguration configuration)
    {
        _baseRepository = baseRepository;
        _configuration = configuration;
    }

    public async Task<JwtWithRefreshDTO> Login(LoginRequest loginRequest)
    {
        AppUser? user = await _baseRepository.GetUserByLogin(loginRequest.Login);
        if (user == null)
        {
            throw new UserNotFoundException(loginRequest.Login);
        }

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            throw new NotAuthorizedException(user.Login);
        }


        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _baseRepository.Update(user);

        return new JwtWithRefreshDTO()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken
        };
    }

    public async Task Register(RegisterRequest registerRequest)
    {
        var userByLogin = await _baseRepository.GetUserByLogin(registerRequest.Login.ToLower());
        if (userByLogin != null)
        {
            throw new UserArleadyExistException();
        }

        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(registerRequest.Password);
        var user = new AppUser()
        {
            Login = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1),
            Role = AppUserRole.User
        };

        await _baseRepository.Add(user);
    }

    public async Task<JwtWithRefreshDTO> Refresh(RefreshTokenRequest refreshRequest)
    {
        AppUser? user = await _baseRepository.GetUserByRefreshToken(refreshRequest.RefreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.IsRefreshTokenExpired)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        Claim[] userclaim = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: userclaim,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);

        return new JwtWithRefreshDTO()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            RefreshToken = user.RefreshToken
        };
    }
}