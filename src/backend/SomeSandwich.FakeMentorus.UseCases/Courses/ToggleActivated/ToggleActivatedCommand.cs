using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.ToggleActivated;

/// <summary>
/// Toggles the activated state of a course.
/// </summary>
public class ToggleActivatedCommand : IRequest
{
    /// <summary>
    /// Course identifier.
    /// </summary>
    public int CourseId { get; set; }
}
