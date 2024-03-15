namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByStudentId;

/// <summary>
/// Grade detail by student id dto.
/// </summary>
public class GradeDetailByStudentIdDto
{
    /// <summary>
    /// Grade id.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Grade Composition id.
    /// </summary>
    public int GradeCompositionId { get; set; }

    /// <summary>
    /// Grade Composition name.
    /// </summary>
    public string GradeCompositionName { get; set; } = string.Empty;

    /// <summary>
    /// Grade value.
    /// </summary>
    public float? GradeValue { get; set; }

    /// <summary>
    /// Is requested.
    /// </summary>
    public bool IsRequested { get; set; }
}
