using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter05.Models;

public class ContactFormModel
{
    [Required(ErrorMessage = "Имя обязательно")]
    [Display(Name = "Имя")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 50 символов")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Сообщение обязательно")]
    [Display(Name = "Сообщение")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Сообщение должно быть от 10 до 500 символов")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Телефон")]
    [Phone(ErrorMessage = "Некорректный формат телефона")]
    public string? Phone { get; set; }
}
