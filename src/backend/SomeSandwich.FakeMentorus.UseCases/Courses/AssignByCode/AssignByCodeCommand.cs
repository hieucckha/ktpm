using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.AssignByCode;

/// <summary>
/// Command to assign a course to a student or a teacher by code.
/// </summary>
public class AssignByCodeCommand : IRequest
{
    /// <summary>
    /// The invite code of the course.
    /// </summary>
    required public string InviteCode { get; set; }
}
