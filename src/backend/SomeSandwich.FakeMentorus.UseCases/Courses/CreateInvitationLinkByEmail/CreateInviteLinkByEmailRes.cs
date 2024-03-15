namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;

/// <summary>
/// Create invitation link by email command result.
/// </summary>
public class CreateInviteLinkByEmailRes
{
    /// <summary>
    /// Invitation token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

}
