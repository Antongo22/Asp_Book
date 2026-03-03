using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter08.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;
}
