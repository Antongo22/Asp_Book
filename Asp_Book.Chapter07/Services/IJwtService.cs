using Asp_Book.Chapter07.Models;

namespace Asp_Book.Chapter07.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    string? ValidateToken(string token);
}
