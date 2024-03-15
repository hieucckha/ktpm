namespace SomeSandwich.FakeMentorus.UseCases.Common;

/// <summary>
/// Dto for notification.
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Title of the notification.
    /// </summary>
    required public string Title { get; set; }

    /// <summary>
    /// Description of the notification.
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of the notification.
    /// </summary>
    required public NotificationType Type { get; set; }

    /// <summary>
    /// Id of the class.
    /// </summary>
    public int? ClassId { get; set; }

    /// <summary>
    /// Id of the request.
    /// </summary>
    public int? RequestId { get; set; }

    /// <summary>
    /// The timestamp when the notification was created.
    /// </summary>
    public DateTime Time { get; set; } = DateTime.Now;
}
