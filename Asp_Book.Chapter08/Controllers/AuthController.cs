using System.Security.Claims;
using Asp_Book.Chapter08.DTOs;
using Asp_Book.Chapter08.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter08.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Проверка: не занято ли имя пользователя
        var existingUser = await _userService.GetUserByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Пользователь с таким именем уже существует" });
        }

        // Создание пользователя
        var user = await _userService.CreateUserAsync(request.Username, request.Email, request.Password);

        // Генерация токенов сразу после регистрации
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

        await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpires);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Role = user.Role,
            AccessTokenExpires = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpires = refreshTokenExpires
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetUserByUsernameAsync(request.Username);
        if (user == null || !_userService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
        }

        // Генерация токенов
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpires = DateTime.UtcNow.AddDays(7); // Refresh token живет 7 дней

        // Сохранение refresh token
        await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpires);

        var response = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Username = user.Username,
            Role = user.Role,
            AccessTokenExpires = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpires = refreshTokenExpires
        };

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        // Проверка валидности refresh token
        if (!await _tokenService.IsRefreshTokenValidAsync(request.RefreshToken))
        {
            return Unauthorized(new { message = "Недействительный refresh token" });
        }

        // Получение refresh token из БД
        var refreshToken = await _tokenService.GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null)
        {
            return Unauthorized(new { message = "Refresh token не найден" });
        }

        // Получение пользователя по ID из refresh token
        var userId = refreshToken.UserId;
        var user = await _userService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return Unauthorized(new { message = "Пользователь не найден" });
        }

        // Отзыв старого refresh token
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

        // Генерация новых токенов
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshTokenExpires = DateTime.UtcNow.AddDays(7);

        // Сохранение нового refresh token
        await _tokenService.SaveRefreshTokenAsync(user.Id, newRefreshToken, newRefreshTokenExpires);

        var response = new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Username = user.Username,
            Role = user.Role,
            AccessTokenExpires = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpires = newRefreshTokenExpires
        };

        return Ok(response);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request)
    {
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
        return Ok(new { message = "Refresh token отозван" });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Невозможно определить пользователя" });

        var revokedCount = await _tokenService.RevokeAllUserTokensAsync(userId);

        return Ok(new 
        { 
            message = $"Все сессии завершены. Отозвано токенов: {revokedCount}" 
        });
    }
}
