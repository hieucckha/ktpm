using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseByUserId;

/// <summary>
///   Get course by user id query.
/// </summary>
public class GetCourseByUserIdQuery : IRequest<IEnumerable<CourseDto>>
{
    /// <summary>
    /// User id.
    /// </summary>
    public int UserId { get; set; }
}
