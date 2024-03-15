using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.LoginUserGoogle;

/// <summary>
/// Login user by google command.
/// </summary>
public class LoginUserGoogleCommand : IRequest<LoginUserCommandResult>
{
    /// <summary>
    /// Credential (Google token).
    /// </summary>
    required public string Credential { get; set; }
}
