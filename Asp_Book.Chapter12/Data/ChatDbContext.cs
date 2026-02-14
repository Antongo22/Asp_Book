using Asp_Book.Chapter12.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp_Book.Chapter12.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatGroup> ChatGroups { get; set; }
}
