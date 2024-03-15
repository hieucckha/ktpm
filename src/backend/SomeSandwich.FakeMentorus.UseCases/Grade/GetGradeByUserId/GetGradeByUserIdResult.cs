using SomeSandwich.FakeMentorus.UseCases.Grade.Common;
using SomeSandwich.FakeMentorus.UseCases.GradeComposition.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByUserId;

/// <summary>
/// Result of retrieving all grades for a specific course.
/// </summary>
public class GetGradeByUserIdResult
{
    /// <summary>
    /// Id of course.
    /// </summary>
    required public int CourseId { get; set; }

    /// <summary>
    /// List of <see cref="GradeCompositionDtos"/> hold the information of grade composition (and order).
    /// </summary>
    required public IReadOnlyList<GradeCompositionDto> GradeCompositionDtos { get; set; }

    /// <summary>
    /// List of grade cell for student.
    /// </summary>
    // public GradeCell Students { get; set; } = new GradeCell() { StudentId = "", StudentName = "", UserId = null };
    public IReadOnlyList<GradeCell> Students { get; set; } = new List<GradeCell>();

}
