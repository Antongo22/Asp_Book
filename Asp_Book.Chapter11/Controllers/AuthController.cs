using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Asp_Book.Chapter11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    // Простое хранилище пользователей (в памяти)
    private static readonly List<UserInfo> _users = new();

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Имя пользователя и пароль обязательны");

        if (_users.Any(u => u.Username == request.Username))
            return BadRequest("Пользователь уже существует");

        _users.Add(new UserInfo
        {
            Username = request.Username,
            Password = request.Password,
            Role = request.Role ?? "User"
        });

        return Ok(new { Message = $"Пользователь {request.Username} зарегистрирован с ролью {request.Role ?? "User"}" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _users.FirstOrDefault(u =>
            u.Username == request.Username && u.Password == request.Password);

        if (user == null)
            return Unauthorized("Неверные учётные данные");

        var token = GenerateJwtToken(user);
        return Ok(new { AccessToken = token, Username = user.Username, Role = user.Role });
    }

    private string GenerateJwtToken(UserInfo user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]
                ?? "MySuperSecretKeyThatIsAtLeast32CharactersLong!"));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "AspBook",
            audience: _config["Jwt:Audience"] ?? "AspBookUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string? Role { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class UserInfo
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "User";
}
