using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;

/// <summary>
///Create invitation link by email command.
/// </summary>
public class CreateInvitationLinkByEmailCommand : IRequest
{
    /// <summary>
    /// Course id to create invitation link.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    ///   Email of user to invite.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
