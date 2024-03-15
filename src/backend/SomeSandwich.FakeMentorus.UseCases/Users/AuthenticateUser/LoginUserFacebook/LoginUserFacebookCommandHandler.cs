using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.LoginUserFacebook;

/// <summary>
/// Handler for <see cref="LoginUserFacebookCommand"/>.
/// </summary>
public class LoginUserFacebookCommandHandler : IRequestHandler<LoginUserFacebookCommand, LoginUserCommandResult>
{
    private readonly ILogger<LoginUserFacebookCommandHandler> logger;
    private readonly SignInManager<User> signInManager;
    private readonly IFacebookClient facebookClient;
    private readonly IAuthenticationTokenService tokenService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="signInManager">Signin manager.</param>
    /// <param name="facebookClient">Facebook client.</param>
    /// <param name="tokenService">Token service.</param>
    public LoginUserFacebookCommandHandler(ILogger<LoginUserFacebookCommandHandler> logger,
        SignInManager<User> signInManager, IFacebookClient facebookClient, IAuthenticationTokenService tokenService)
    {
        this.logger = logger;
        this.signInManager = signInManager;
        this.facebookClient = facebookClient;
        this.tokenService = tokenService;
    }

    /// <inheritdoc />
    public async Task<LoginUserCommandResult> Handle(LoginUserFacebookCommand command,
        CancellationToken cancellationToken)
    {
        var data = await facebookClient.GetCurrentUserInformationAsync(command.AccessToken, cancellationToken);
        if (data.Email is null)
        {
            throw new DomainException($"Cannot get email from user.");
        }

        var user = await signInManager.UserManager.FindByEmailAsync(data.Email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {data.Email} not found.");
        }

        await signInManager.SignInAsync(user, true);

        logger.LogInformation("User with email {Email} has logged in", user.Email);

        // Update last login date.
        user.LastLogin = DateTime.UtcNow;
        await signInManager.UserManager.UpdateAsync(user);

        // Combine refresh token with user id.
        var principal = await signInManager.CreateUserPrincipalAsync(user);

        // Give token.
        return new LoginUserCommandResult
        {
            UserId = user.Id, TokenModel = TokenModelGenerator.Generate(tokenService, principal.Claims)
        };
    }
}
