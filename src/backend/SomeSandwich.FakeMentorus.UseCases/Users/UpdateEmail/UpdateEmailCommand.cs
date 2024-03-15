using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateEmail;

/// <summary>
/// Command to update email.
/// </summary>
public class UpdateEmailCommand : IRequest
{
    /// <summary>
    /// User id.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    required public string Email { get; set; }
}
