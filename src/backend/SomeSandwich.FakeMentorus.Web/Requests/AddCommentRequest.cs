namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Add comment request.
/// </summary>
public class AddCommentRequest
{
    /// <summary>
    /// Comment details.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
