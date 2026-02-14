namespace Asp_Book.Chapter08.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
