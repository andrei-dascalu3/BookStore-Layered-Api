using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

using Microsoft.IdentityModel.Tokens;

namespace BookStore.Presentation.Services;

internal sealed class AuthService : IAuthService
{
    private const string RefreshTokenCookieName = "refreshToken";

    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        IConfiguration configuration,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _httpContextAccessor = httpContextAccessor;
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
            PasswordHash = GetHash(request.Password),
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

        var hash = GetHash(request.Password);
        if (user.PasswordHash != hash)
        {
            return null;
        }

        return GenerateResponse(user);
    }

    public AuthResponse? Refresh()
    {
        var refreshTokenValue = _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenCookieName];
        if (string.IsNullOrEmpty(refreshTokenValue))
        {
            return null;
        }

        var refreshTokenHash = GetHash(refreshTokenValue);
        var refreshToken = _refreshTokenRepository.GetByHash(refreshTokenHash);
        if (refreshToken == null || refreshToken.TokenExpiry < DateTime.UtcNow)
        {
            return null;
        }

        var user = _userRepository.GetById(refreshToken.UserId);
        if (user == null)
        {
            return null;
        }

        return GenerateResponse(user);
    }

    public bool Logout(string username)
    {
        var user = _userRepository.GetByUsername(username);
        if (user == null)
        {
            return false;
        }

        this._refreshTokenRepository.DeleteByUserId(user.Id);
        DeleteRefreshTokenCookie();

        return true;
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

        var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenExpirationDays = _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays");
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);
        var refreshToken = new RefreshToken
        {
            Hash = GetHash(refreshTokenValue),
            TokenExpiry = refreshTokenExpiry,
            UserId = user.Id
        };
        _refreshTokenRepository.DeleteByUserId(user.Id);
        _refreshTokenRepository.Create(refreshToken);

        SetRefreshTokenCookie(refreshTokenValue, refreshTokenExpiry);

        return new AuthResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    private void SetRefreshTokenCookie(string value, DateTime expiry)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiry
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenCookieName, value, cookieOptions);
    }

    private void DeleteRefreshTokenCookie()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenCookieName);
    }

    private static string GetHash(string password)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
}
