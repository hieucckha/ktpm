using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.LockoutUser.UnlockUser;

/// <summary>
/// Command to unlock the user with id.
/// </summary>
public class UnlockUserCommand : IRequest
{
    /// <summary>
    /// Id of user.
    /// </summary>
    public int UserId { get; set; }
}
