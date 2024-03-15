using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.MappingStudentId;

/// <summary>
///    This is a command to map a student id to a user.
/// </summary>
public class MappingStudentIdCommand : IRequest
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
