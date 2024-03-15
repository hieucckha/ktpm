using SomeSandwich.FakeMentorus.Domain.Grade;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.Common;

/// <summary>
/// GradeComposition DTO.
/// </summary>
public class GradeCompositionDto
{
    /// <inheritdoc cref="GradeComposition.Id"/>
    public int Id { get; set; }

    /// <inheritdoc cref="GradeComposition.Name"/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="GradeComposition.CourseId"/>
    public int CourseId { get; set; }

    /// <inheritdoc cref="GradeComposition.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="GradeComposition.GradeScale"/>
    public int GradeScale { get; set; }

    /// <inheritdoc cref="GradeComposition.Order"/>
    public int Order { get; set; }

    /// <inheritdoc cref="GradeComposition.IsFinal"/>
    public bool IsFinal { get; set; }

    /// <inheritdoc cref="GradeComposition.CreatedAt"/>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <inheritdoc cref="GradeComposition.UpdatedAt"/>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
