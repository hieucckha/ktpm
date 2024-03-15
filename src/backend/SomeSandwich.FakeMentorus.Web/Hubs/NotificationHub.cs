using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.Web.Hubs;

/// <summary>
///
/// </summary>
public class NotificationHub : Hub, INotificationService
{
    private readonly IHubContext<NotificationHub> hubContext;

    public NotificationHub(IHubContext<NotificationHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    public async Task SendNotification(string user, string message, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.SendAsync("ReceiveNotification", user, message, cancellationToken);
    }
}
