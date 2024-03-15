namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Create invitation link by email command.
/// </summary>
public class InviteByEmailRequest
{
    /// <summary>
    /// Email of user to invite.
    /// </summary>
    required public string Email { get; set; }
}
