using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter07.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
