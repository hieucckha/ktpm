namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
///
/// </summary>
public interface INotificationService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendNotification(string user, string message, CancellationToken cancellationToken);
}
