using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.GetGradeCompositionById;

/// <summary>
/// Get grade composition by id query.
/// </summary>
public class GetGradeCompositionByIdQuery : IRequest<GradeCompositionDetailDto>
{
    /// <summary>
    /// Grade composition id.
    /// </summary>
    public int Id { get; set; }
}
