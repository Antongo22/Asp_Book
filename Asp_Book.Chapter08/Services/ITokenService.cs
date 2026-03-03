using Asp_Book.Chapter08.Models;

namespace Asp_Book.Chapter08.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expires);
    Task RevokeRefreshTokenAsync(string token);
    Task<bool> IsRefreshTokenValidAsync(string token);
    Task<int> RevokeAllUserTokensAsync(int userId);
}
