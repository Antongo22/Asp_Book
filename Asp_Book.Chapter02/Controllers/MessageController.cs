using Asp_Book.Chapter02.Services;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Book.Chapter02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMessageService messageService, ILogger<MessageController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// Получить приветственное сообщение (демонстрация DI)
    /// </summary>
    [HttpGet("greet/{name}")]
    public IActionResult Greet(string name)
    {
        var message = _messageService.GetMessage(name);
        return Ok(new { message });
    }

    /// <summary>
    /// Добавить сообщение в лог
    /// </summary>
    [HttpPost("log")]
    public IActionResult LogMessage([FromBody] LogMessageRequest request)
    {
        _messageService.LogMessage(request.Message);
        return Ok(new { success = true, message = "Сообщение добавлено в лог" });
    }
}

public class LogMessageRequest
{
    public string Message { get; set; } = string.Empty;
}
