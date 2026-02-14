namespace Asp_Book.Chapter12.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? GroupId { get; set; }
}
