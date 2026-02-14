using Asp_Book.Chapter12.Models;
using Asp_Book.Chapter12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        var message = await _chatService.SendMessageAsync(
            request.Text, 
            request.Sender, 
            request.GroupId);
        return Ok(message);
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessages([FromQuery] int? groupId = null)
    {
        var messages = await _chatService.GetMessagesAsync(groupId);
        return Ok(messages);
    }

    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        var group = await _chatService.CreateGroupAsync(request.Name);
        return Ok(group);
    }
}

public class SendMessageRequest
{
    public string Text { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public int? GroupId { get; set; }
}

public class CreateGroupRequest
{
    public string Name { get; set; } = string.Empty;
}
