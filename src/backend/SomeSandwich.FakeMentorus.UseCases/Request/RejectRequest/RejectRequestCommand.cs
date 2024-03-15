using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Request.RejectRequest;

/// <summary>
/// Command for rejecting request.
/// </summary>
public class RejectRequestCommand: IRequest
{
    /// <summary>
    /// Request id.
    /// </summary>
    public int RequestId { get; set; }
}
