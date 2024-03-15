using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.DeleteGradeComposition;

/// <summary>
/// The command to delete a grade composition.
/// </summary>
public class DeleteGradeCompositionCommand : IRequest
{
    /// <summary>
    /// The id of the grade composition to delete.
    /// </summary>
    required public int GradeCompositionId { get; set; }
}
