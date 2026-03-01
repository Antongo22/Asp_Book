using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter05.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя пользователя должно быть от 3 до 20 символов")]
    [Display(Name = "Имя пользователя")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Подтверждение пароля обязательно")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [Display(Name = "Подтверждение пароля")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Возраст обязателен")]
    [Range(18, 120, ErrorMessage = "Возраст должен быть от 18 до 120 лет")]
    [Display(Name = "Возраст")]
    public int Age { get; set; }
}
