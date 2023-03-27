using backend.BLL.Common.VMs.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace backend.API.Hubs;

[EnableCors("signalr")]
public class ConsoleHub : Hub
{
    [Authorize(Roles = "Admin")]
    public async Task Execute(string command)
    {
        await SendMessageAsync($"Unknown command {command}!", "red");
    }

    private async Task SendMessageAsync(string message, string color = "magenta")
    {
        var msg = new ChatMessageVM
        {
            Color = color,
            Date = DateTime.Now.ToString("MM/dd/yyyy H:mm"),
            Message = message,
            FullName = "@Console"
        };
        await Clients.Caller.SendAsync("receiveMessage", msg);
    }
}