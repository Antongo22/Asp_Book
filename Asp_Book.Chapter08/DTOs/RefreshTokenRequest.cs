using System.ComponentModel.DataAnnotations;

namespace Asp_Book.Chapter08.DTOs;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
