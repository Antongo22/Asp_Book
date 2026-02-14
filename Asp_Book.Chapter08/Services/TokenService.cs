using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Asp_Book.Chapter08.Data;
using Asp_Book.Chapter08.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Asp_Book.Chapter08.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
        _secretKey = _configuration["Jwt:SecretKey"] ?? "MySuperSecretKeyThatIsAtLeast32CharactersLong!";
        _issuer = _configuration["Jwt:Issuer"] ?? "AspBook";
        _audience = _configuration["Jwt:Audience"] ?? "AspBookUsers";
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Access token - короткий срок
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expires)
    {
        var token = new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            Expires = expires,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsRefreshTokenValidAsync(string token)
    {
        var refreshToken = await GetRefreshTokenAsync(token);
        return refreshToken != null 
            && !refreshToken.IsRevoked 
            && refreshToken.Expires > DateTime.UtcNow;
    }
}
