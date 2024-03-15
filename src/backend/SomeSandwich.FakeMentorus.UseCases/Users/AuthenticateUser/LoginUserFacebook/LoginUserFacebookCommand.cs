using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.LoginUserFacebook;

/// <summary>
/// Login user by facebook command.
/// </summary>
public class LoginUserFacebookCommand : IRequest<LoginUserCommandResult>
{
    /// <summary>
    /// Access token from facebook.
    /// </summary>
    required public string AccessToken { get; set; }
}
