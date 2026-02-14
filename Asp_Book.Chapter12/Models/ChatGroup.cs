namespace Asp_Book.Chapter12.Models;

public class ChatGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Message> Messages { get; set; } = new();
}
