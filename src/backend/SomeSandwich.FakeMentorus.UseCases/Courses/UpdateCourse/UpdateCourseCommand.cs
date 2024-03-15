using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.UpdateCourse;

/// <summary>
///Update course command.
/// </summary>
public class UpdateCourseCommand : IRequest
{
    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }
}
