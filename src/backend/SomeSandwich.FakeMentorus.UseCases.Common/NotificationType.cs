namespace SomeSandwich.FakeMentorus.UseCases.Common;

/// <summary>
/// Type of notification.
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Notification for grade composition is finalized.
    /// </summary>
    FinalizedGradeComposition,

    /// <summary>
    /// Notification for create a new comment.
    /// </summary>
    CreateComment,

    /// <summary>
    /// Notification for create a new request.
    /// </summary>
    CreateRequest,

    /// <summary>
    /// Notification for approve a request.
    /// </summary>
    ApproveRequest,

    /// <summary>
    /// Notification for reject a request.
    /// </summary>
    RejectRequest,
}
