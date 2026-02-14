using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Asp_Book.Chapter11.Middleware;

public class AuthorizedWebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Dictionary<string, WebSocket> _connections = new();
    private static readonly Dictionary<string, string> _userConnections = new();

    public AuthorizedWebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            // Проверка авторизации через Query параметр
            var token = context.Request.Query["access_token"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            // В реальном приложении здесь должна быть валидация JWT токена
            // Для упрощения пропускаем подключение
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var connectionId = Guid.NewGuid().ToString();
            _connections[connectionId] = webSocket;

            await HandleWebSocket(webSocket, connectionId);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocket(WebSocket webSocket, string connectionId)
    {
        var buffer = new byte[1024 * 4];

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await BroadcastMessageAsync($"{connectionId}: {message}");
                }
            }
        }
        finally
        {
            _connections.Remove(connectionId);
        }
    }

    private async Task BroadcastMessageAsync(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tasks = _connections.Values
            .Where(ws => ws.State == WebSocketState.Open)
            .Select(ws => ws.SendAsync(
                new ArraySegment<byte>(messageBytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None));

        await Task.WhenAll(tasks);
    }
}
