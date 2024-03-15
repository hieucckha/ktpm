namespace SomeSandwich.FakeMentorus.UseCases.Grade.Common;

/// <summary>
/// Dto for grade.
/// </summary>
public class GradeDto
{
    /// <summary>
    /// Grade id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Grade Composition id.
    /// </summary>
    public int GradeCompositionId { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string StudentId { get; set; }

    /// <summary>
    /// Student name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Grade value.
    /// </summary>
    public float GradeValue { get; set; }

    /// <summary>
    /// Is requested.
    /// </summary>
    public bool IsRequested { get; set; }
}
