namespace Asp_Book.Chapter02.Services;

public class MessageService : IMessageService
{
    private readonly ILogger<MessageService> _logger;
    private readonly List<string> _messages = new();

    public MessageService(ILogger<MessageService> logger)
    {
        _logger = logger;
    }

    public string GetMessage(string name)
    {
        var message = $"Привет, {name}! Это сообщение от MessageService.";
        _logger.LogInformation("Сгенерировано сообщение для {Name}", name);
        return message;
    }

    public void LogMessage(string message)
    {
        _messages.Add($"{DateTime.Now:HH:mm:ss} - {message}");
        _logger.LogInformation("Добавлено сообщение: {Message}", message);
    }

    public List<string> GetAllMessages() => _messages;
}
