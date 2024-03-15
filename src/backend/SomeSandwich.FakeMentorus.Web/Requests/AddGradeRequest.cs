namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Add grade request.
/// </summary>
public class AddGradeRequest
{
    /// <summary>
    /// Grade value.
    /// </summary>
    required public float GradeValue { get; set; }
}
