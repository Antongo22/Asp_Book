using Asp_Book.Chapter07.DTOs;
using Asp_Book.Chapter07.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter07.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (_userService.GetUserByUsername(request.Username) != null)
        {
            return BadRequest(new { message = "Пользователь с таким именем уже существует" });
        }

        if (_userService.GetUserByEmail(request.Email) != null)
        {
            return BadRequest(new { message = "Пользователь с таким email уже существует" });
        }

        var user = _userService.CreateUser(request.Username, request.Email, request.Password);
        var token = _jwtService.GenerateToken(user);

        var response = new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.GetUserByUsername(request.Username);
        if (user == null || !_userService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
        }

        var token = _jwtService.GenerateToken(user);

        var response = new AuthResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        return Ok(response);
    }
}
