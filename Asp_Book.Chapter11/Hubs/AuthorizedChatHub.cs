using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Asp_Book.Chapter11.Hubs;

[Authorize] // Требует авторизации для подключения к Hub
public class AuthorizedChatHub : Hub
{
    // Отправка сообщения всем авторизованным пользователям
    public async Task SendToAll(string message)
    {
        var username = Context.User?.Identity?.Name ?? "Unknown";
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }

    // Отправка сообщения конкретному пользователю по ConnectionId
    public async Task SendToUser(string connectionId, string message)
    {
        var username = Context.User?.Identity?.Name ?? "Unknown";
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", username, message);
    }

    // Отправка только администраторам
    [Authorize(Roles = "Admin")]
    public async Task SendToAdmins(string message)
    {
        var username = Context.User?.Identity?.Name ?? "Unknown";
        await Clients.Group("Admins").SendAsync("ReceiveAdminMessage", username, message);
    }

    public override async Task OnConnectedAsync()
    {
        // Добавление в группу по роли
        if (Context.User?.IsInRole("Admin") == true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        }

        var username = Context.User?.Identity?.Name ?? "Unknown";
        await Clients.All.SendAsync("UserConnected", username);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User?.IsInRole("Admin") == true)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
        }

        var username = Context.User?.Identity?.Name ?? "Unknown";
        await Clients.All.SendAsync("UserDisconnected", username);
        await base.OnDisconnectedAsync(exception);
    }
}
