using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.LockoutUser.LockUser;

/// <summary>
/// Command to unlock the user with id.
/// </summary>
public class LockUserCommand : IRequest
{
    /// <summary>
    /// Id of user.
    /// </summary>
    public int UserId { get; set; }
}
