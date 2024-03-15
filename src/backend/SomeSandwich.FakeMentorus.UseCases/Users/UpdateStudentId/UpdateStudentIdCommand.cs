using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateStudentId;

/// <summary>
/// Command to update student id.
/// </summary>
public class UpdateStudentIdCommand : IRequest
{
    /// <summary>
    /// User id.
    /// </summary>
    required public int UserId { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string? StudentId { get; set; }
}
