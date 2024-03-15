using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateCourse;

/// <summary>
/// Command to create a new course.
/// </summary>
public class CreateCourseCommand : IRequest<int>
{
    /// <summary>
    /// Course name.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Course description.
    /// </summary>
    public string? Description { get; set; }
}
