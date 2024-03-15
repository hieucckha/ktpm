using SomeSandwich.FakeMentorus.UseCases.GradeComposition.SortGradeComposition;

namespace SomeSandwich.FakeMentorus.Web.Requests;

public class SortGradeCompositionRequest
{
    /// <summary>
    /// List of grade composition to sort.
    /// </summary>
    public ICollection<SortCompositionDto> GradeCompositions { get; set; } = new List<SortCompositionDto>();
}
