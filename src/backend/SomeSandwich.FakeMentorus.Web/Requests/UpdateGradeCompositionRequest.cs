namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Represents the request to update a grade composition.
/// </summary>
public class UpdateGradeCompositionRequest
{
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
