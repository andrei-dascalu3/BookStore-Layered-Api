using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using BookStore.Presentation.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Presentation.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public AuthResponse? Register(RegisterRequest request)
    {
        var user = _userRepository.GetByUsername(request.Username);
        if (user != null)
        {
            return null;
        }

        user = new User
        {
            Username = request.Username,
            PasswordHash = UserRepository.HashPassword(request.Password),
            Role = "User"
        };

        _userRepository.Add(user);

        return GenerateResponse(user);
    }

    public AuthResponse? Login(LoginRequest request)
    {
        var user = _userRepository.GetByUsername(request.Username);
        if (user == null)
        {
            return null;
        }

        var hash = UserRepository.HashPassword(request.Password);
        if (user.PasswordHash != hash)
        {
            return null;
        }

        return GenerateResponse(user);
    }

    private AuthResponse GenerateResponse(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var expirationMinutes = _configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}
