using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.AssignByEmail;

/// <summary>
/// Assigns a user to a course by token.
/// </summary>
public class AssignByEmailCommand : IRequest
{
    /// <summary>
    /// Token to assign a user to a course.
    /// </summary>
    required public string Token { get; set; }
}
