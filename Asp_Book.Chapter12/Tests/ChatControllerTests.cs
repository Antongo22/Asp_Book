using Asp_Book.Chapter12.Controllers;
using Asp_Book.Chapter12.Data;
using Asp_Book.Chapter12.Models;
using Asp_Book.Chapter12.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Asp_Book.Chapter12.Tests;

public class ChatControllerTests
{
    private ChatDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ChatDbContext(options);
    }

    [Fact]
    public async Task SendMessage_ShouldReturnOk()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);
        var controller = new ChatController(service);

        var request = new SendMessageRequest
        {
            Text = "Привет",
            Sender = "User1"
        };

        // Act
        var result = await controller.SendMessage(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var message = Assert.IsType<Message>(okResult.Value);
        Assert.Equal("Привет", message.Text);
        Assert.Equal("User1", message.Sender);
    }

    [Fact]
    public async Task GetMessages_ShouldReturnMessages()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);
        var controller = new ChatController(service);

        // Добавляем тестовые сообщения
        await service.SendMessageAsync("Сообщение 1", "User1");
        await service.SendMessageAsync("Сообщение 2", "User2");

        // Act
        var result = await controller.GetMessages();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var messages = Assert.IsAssignableFrom<List<Message>>(okResult.Value);
        Assert.Equal(2, messages.Count);
    }

    [Fact]
    public async Task CreateGroup_ShouldReturnGroup()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new ChatService(context);
        var controller = new ChatController(service);

        var request = new CreateGroupRequest { Name = "Группа 1" };

        // Act
        var result = await controller.CreateGroup(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var group = Assert.IsType<ChatGroup>(okResult.Value);
        Assert.Equal("Группа 1", group.Name);
    }
}
