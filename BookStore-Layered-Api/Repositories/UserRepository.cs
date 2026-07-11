using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;
using System.Security.Cryptography;
using System.Text;

namespace BookStore.Presentation.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private static readonly List<User> _users =
    [
        new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("admin123"))),
            Role = "Admin"
        }
    ];

    private static int _nextId = 2;

    public User? GetByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public User Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }
}
