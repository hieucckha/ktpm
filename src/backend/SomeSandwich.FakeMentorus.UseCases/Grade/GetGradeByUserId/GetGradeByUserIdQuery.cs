using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByUserId;

/// <summary>
/// Command for retrieving all grades for a specific course.
/// </summary>
public class GetGradeByUserIdQuery : IRequest<GetGradeByUserIdResult>
{
    /// <summary>
    /// The student id.
    /// </summary>
    required public string StudentId { get; set; }

    /// <summary>
    /// The course id.
    /// </summary>
    required public int CourseId { get; set; }
}
