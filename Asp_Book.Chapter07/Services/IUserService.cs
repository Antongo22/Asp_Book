using Asp_Book.Chapter07.Models;

namespace Asp_Book.Chapter07.Services;

public interface IUserService
{
    User? GetUserByUsername(string username);
    User? GetUserByEmail(string email);
    User CreateUser(string username, string email, string password);
    bool VerifyPassword(string password, string passwordHash);
}
