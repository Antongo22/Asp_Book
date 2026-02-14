using Asp_Book.Chapter08.Models;

namespace Asp_Book.Chapter08.Services;

public interface IUserService
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(string username, string email, string password);
    bool VerifyPassword(string password, string passwordHash);
}
