namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Join course by email request.
/// </summary>
public class JoinCourseByEmailRequest
{
    /// <summary>
    /// Token of invitation.
    /// </summary>
    required public string Token { get; set; }
}
