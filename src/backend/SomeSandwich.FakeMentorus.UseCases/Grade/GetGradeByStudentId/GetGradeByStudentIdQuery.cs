using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByStudentId;

/// <summary>
/// Command to get a grade by student id in a course.
/// </summary>
public class GetGradeByStudentIdQuery : IRequest<GetGradeByStudentIdDto>
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
