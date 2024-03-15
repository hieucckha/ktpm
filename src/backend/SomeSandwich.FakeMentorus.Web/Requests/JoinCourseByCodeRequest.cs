namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Join course by code request.
/// </summary>
public class JoinCourseByCodeRequest
{
    /// <summary>
    /// Invitation code.
    /// </summary>
    required public string InviteCode { get; set; }
}
