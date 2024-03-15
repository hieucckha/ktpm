using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseById;

/// <summary>
/// Get course by id query.
/// </summary>
public class GetCourseByIdQuery : IRequest<CourseDetailDto>
{
    /// <summary>
    /// Gets or sets course id.
    /// </summary>
    public int CourseId { get; set; }
}
