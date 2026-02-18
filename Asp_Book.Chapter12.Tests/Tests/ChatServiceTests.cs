using Asp_Book.Chapter12.Data;
using Asp_Book.Chapter12.Models;
using Asp_Book.Chapter12.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Asp_Book.Chapter12.Tests;

public class ChatServiceTests
{
    private ChatDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ChatDbContext(options);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldSaveMessage()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);

        // Act
        var message = await service.SendMessageAsync("Тест", "User1");

        // Assert
        Assert.NotNull(message);
        Assert.Equal("Тест", message.Text);
        Assert.Equal("User1", message.Sender);
        Assert.True(message.Id > 0);
    }

    [Fact]
    public async Task GetMessagesAsync_ShouldReturnMessages()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);

        await service.SendMessageAsync("Сообщение 1", "User1");
        await service.SendMessageAsync("Сообщение 2", "User2");

        // Act
        var messages = await service.GetMessagesAsync();

        // Assert
        Assert.Equal(2, messages.Count);
        Assert.Equal("Сообщение 1", messages[0].Text);
        Assert.Equal("Сообщение 2", messages[1].Text);
    }

    [Fact]
    public async Task GetMessagesAsync_WithGroupId_ShouldReturnGroupMessages()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);

        var group = await service.CreateGroupAsync("Группа 1");
        await service.SendMessageAsync("Сообщение в группе", "User1", group.Id);
        await service.SendMessageAsync("Сообщение вне группы", "User2", null);

        // Act
        var groupMessages = await service.GetMessagesAsync(group.Id);
        var allMessages = await service.GetMessagesAsync(null);

        // Assert
        Assert.Single(groupMessages);
        Assert.Equal(2, allMessages.Count);
    }
}
