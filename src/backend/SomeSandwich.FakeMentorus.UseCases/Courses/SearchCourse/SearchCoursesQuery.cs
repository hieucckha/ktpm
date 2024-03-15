using MediatR;
using Saritasa.Tools.Common.Pagination;
using SomeSandwich.FakeMentorus.UseCases.Common.Pagination;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.SearchCourse;

/// <summary>
/// Search courses query.
/// </summary>
public class SearchCoursesQuery : PageQueryFilter, IRequest<PagedList<CourseDto>>
{
    /// <summary>
    ///     Gets or sets user id.
    /// </summary>
    public int? UserId { get; set; }
}
