using MediatR;
using SomeSandwich.FakeMentorus.Domain.Grade;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.CreateGradeComposition;

/// <summary>
/// Command for creating a grade composition.
/// </summary>
public class CreateGradeCompositionCommand : IRequest<int>
{
    /// <inheritdoc cref="GradeComposition.Name"/>
    required public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="GradeComposition.CourseId"/>
    required public int CourseId { get; set; }

    /// <inheritdoc cref="GradeComposition.Description"/>
    public string? Description { get; set; }

    /// <inheritdoc cref="GradeComposition.GradeScale"/>
    required public int GradeScale { get; set; }
}
