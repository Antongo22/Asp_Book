using System.Security.Cryptography;
using System.Text;
using Asp_Book.Chapter07.Models;

namespace Asp_Book.Chapter07.Services;

public class UserService : IUserService
{
    // В реальном приложении здесь будет работа с БД
    private static readonly List<User> _users = new();

    public User? GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username == username);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public User CreateUser(string username, string email, string password)
    {
        var user = new User
        {
            Id = _users.Count + 1,
            Username = username,
            Email = email,
            PasswordHash = HashPassword(password),
            Role = "User"
        };

        _users.Add(user);
        return user;
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == passwordHash;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
