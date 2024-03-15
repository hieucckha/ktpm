namespace SomeSandwich.FakeMentorus.UseCases.Grade.Common;

/// <summary>
/// Cell containing information about a student and their associated grades.
/// </summary>
public class GradeCell
{
    /// <summary>
    /// Id of student.
    /// </summary>
    required public string StudentId { get; set; }

    /// <summary>
    /// Name of student.
    /// </summary>
    public string? StudentName { get; set; }

    /// <summary>
    /// Id of user.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// List of <see cref="GradeCellDto"/>.
    /// </summary>
    public IReadOnlyList<GradeCellDto> GradeDto { get; set; } = new List<GradeCellDto>();
}
