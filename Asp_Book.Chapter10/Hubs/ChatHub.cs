using Microsoft.AspNetCore.SignalR;

namespace Asp_Book.Chapter10.Hubs;

public class ChatHub : Hub
{
    // Отправка сообщения всем подключенным клиентам
    public async Task SendToAll(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Отправка сообщения конкретному клиенту по ConnectionId
    public async Task SendToUser(string connectionId, string user, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
    }

    // Отправка сообщения всем, кроме отправителя
    public async Task SendToOthers(string user, string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", user, message);
    }

    // Подключение клиента
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    // Отключение клиента
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
