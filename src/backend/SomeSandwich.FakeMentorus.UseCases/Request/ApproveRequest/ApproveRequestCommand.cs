using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Request.ApproveRequest;

/// <summary>
/// Approve request command.
/// </summary>
public class ApproveRequestCommand : IRequest
{
    /// <summary>
    /// Request id.
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// Grade value.
    /// </summary>
    public float GradeValue { get; set; }
}
