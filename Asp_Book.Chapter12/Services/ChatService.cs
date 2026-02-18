using Asp_Book.Chapter12.Data;
using Asp_Book.Chapter12.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp_Book.Chapter12.Services;

public class ChatService : IChatService
{
    private readonly ChatDbContext _context;

    public ChatService(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<Message> SendMessageAsync(string text, string sender, int? groupId = null)
    {
        var message = new Message
        {
            Text = text,
            Sender = sender,
            GroupId = groupId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<Message>> GetMessagesAsync(int? groupId = null)
    {
        var query = _context.Messages.AsQueryable();
        
        if (groupId.HasValue)
        {
            query = query.Where(m => m.GroupId == groupId);
        }

        return await query.OrderBy(m => m.CreatedAt).ToListAsync();
    }

    public async Task<ChatGroup> CreateGroupAsync(string name)
    {
        var group = new ChatGroup { Name = name };
        _context.ChatGroups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }
}
