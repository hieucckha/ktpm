using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.LoginUserGoogle;

/// <summary>
/// Handler for <see cref="LoginUserGoogleCommand"/>.
/// </summary>
public class LoginUserGoogleCommandHandler : IRequestHandler<LoginUserGoogleCommand, LoginUserCommandResult>
{
    private readonly ILogger<LoginUserGoogleCommandHandler> logger;
    private readonly SignInManager<User> signInManager;
    private readonly IAuthenticationTokenService tokenService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="signInManager">Sign in manager.</param>
    /// <param name="tokenService">Token service.</param>
    public LoginUserGoogleCommandHandler(ILogger<LoginUserGoogleCommandHandler> logger,
        SignInManager<User> signInManager, IAuthenticationTokenService tokenService)
    {
        this.logger = logger;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    /// <inheritdoc />
    public async Task<LoginUserCommandResult> Handle(LoginUserGoogleCommand command,
        CancellationToken cancellationToken)
    {
        var validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { "856018113300-i5i4padrol9e1nocd5ibvm2k1uuh70rm.apps.googleusercontent.com" },
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(command.Credential, validationSettings);
        var user = await signInManager.UserManager.FindByEmailAsync(payload.Email);

        if (user is null)
        {
            throw new NotFoundException($"User with email {payload.Email} not found.");
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
