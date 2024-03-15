namespace SomeSandwich.FakeMentorus.Domain.Request;

/// <summary>
/// Request status.
/// </summary>
public enum RequestStatus
{
    /// <summary>
    /// Pending request.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Approved request.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Rejected request.
    /// </summary>
    Rejected = 2
}
