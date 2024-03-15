using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.SortGradeComposition;

/// <summary>
/// Command to sort grade composition.
/// </summary>
public class SortGradeCompositionCommand : IRequest
{
    /// <summary>
    /// List of grade composition to sort.
    /// </summary>
    public ICollection<SortCompositionDto> GradeCompositions { get; set; } = new List<SortCompositionDto>();
}
