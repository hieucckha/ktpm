using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.GetGradeCompositionById;

/// <summary>
/// GradeComposition entity.
/// </summary>
public class GradeCompositionDetailDto
{
    /// <summary>
    /// Grade Composition id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Grade Composition name.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Description of the Grade Composition.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Grade Scale of the Grade Composition.
    /// </summary>
    required public int GradeScale { get; set; }

    /// <summary>
    ///    Is Grade Composition final?.
    /// </summary>
    required public bool IsFinal { get; set; }

    /// <summary>
    /// Grade Composition order.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Grade Composition grades.
    /// </summary>
    public ICollection<GradeDto> Grades { get; set; } = null!;

    /// <summary>
    /// Grade Composition start date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Grade Composition update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
