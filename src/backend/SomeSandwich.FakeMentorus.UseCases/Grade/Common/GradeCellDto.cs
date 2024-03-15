namespace SomeSandwich.FakeMentorus.UseCases.Grade.Common;

/// <summary>
/// Dto for grade in cell (only use in get all grade).
/// </summary>
public class GradeCellDto
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
    /// Grade value.
    /// </summary>
    public float? GradeValue { get; set; }

    /// <summary>
    /// Is requested.
    /// </summary>
    public bool IsRequested { get; set; }
}
