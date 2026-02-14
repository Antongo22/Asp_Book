using Asp_Book.Chapter12.Models;

namespace Asp_Book.Chapter12.Services;

public interface IChatService
{
    Task<Message> SendMessageAsync(string text, string sender, int? groupId = null);
    Task<List<Message>> GetMessagesAsync(int? groupId = null);
    Task<ChatGroup> CreateGroupAsync(string name);
}
