using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter07.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; } = string.Empty;
}
