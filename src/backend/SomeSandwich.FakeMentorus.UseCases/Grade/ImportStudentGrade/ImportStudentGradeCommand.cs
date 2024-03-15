using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentGrade;

/// <summary>
/// Command to import student grade from template file.
/// </summary>
public class ImportStudentGradeCommand : IRequest
{
    /// <summary>
    /// Id of course.
    /// </summary>
    required public int CourseId { get; set; }

    /// <summary>
    /// Student grade template file.
    /// </summary>
    required public Stream FileContent { get; set; }
}
