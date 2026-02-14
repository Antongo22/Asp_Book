using System.Net.WebSockets;
using System.Text;

namespace Asp_Book.Chapter09.Middleware;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Dictionary<string, WebSocket> _connections = new();

    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
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
                    
                    // Отправка сообщения всем подключенным клиентам
                    await BroadcastMessageAsync($"{connectionId}: {message}");
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Connection closed",
                        CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
        }
        finally
        {
            _connections.Remove(connectionId);
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Connection closed",
                    CancellationToken.None);
            }
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

    public static async Task SendToConnectionAsync(string connectionId, string message)
    {
        if (_connections.TryGetValue(connectionId, out var webSocket) 
            && webSocket.State == WebSocketState.Open)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(
                new ArraySegment<byte>(messageBytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
