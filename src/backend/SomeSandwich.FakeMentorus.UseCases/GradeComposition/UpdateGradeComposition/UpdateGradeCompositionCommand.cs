using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.UpdateGradeComposition;

/// <summary>
///    Represents the command to update a grade composition.
/// </summary>
public class UpdateGradeCompositionCommand : IRequest
{
    /// <summary>
    /// Grade Composition Id.
    /// </summary>
    required public int GradeCompositionId { get; set; }

    /// <summary>
    /// Grade Composition name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description of the Grade Composition.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Grade Scale of the Grade Composition.
    /// </summary>
    public int? GradeScale { get; set; }
}
