using System.Security.Cryptography;
using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

namespace BookStore.Presentation.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private static readonly List<User> _users =
    [
        new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = HashPassword("admin123"),
            Role = "Admin"
        }
    ];

    private static int _nextId = 2;

    public User? GetByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public User? GetByRefreshToken(string refreshToken)
    {
        return _users.FirstOrDefault(u => u.RefreshToken == refreshToken);
    }

    public User? GetByRefreshToken(string refreshToken) =>
        _users.FirstOrDefault(u => u.RefreshToken == refreshToken);

    public User Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }

    public static string HashPassword(string password)
    {
        var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
}
