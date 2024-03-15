using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ResetPasswordUser.GenerateTokenResetPasswordUser;

/// <summary>
/// Handler for <see cref="GenerateTokenResetPasswordUserCommand"/>.
/// </summary>
public class GenerateTokenResetPasswordUserCommandHandler : IRequestHandler<GenerateTokenResetPasswordUserCommand>
{
    private readonly UserManager<User> userManager;
    private readonly IEmailSender emailSender;
    private readonly IAppSettings appSettings;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="emailSender">Email sender service.</param>
    /// <param name="appSettings"></param>
    public GenerateTokenResetPasswordUserCommandHandler(UserManager<User> userManager, IEmailSender emailSender, IAppSettings appSettings)
    {
        this.userManager = userManager;
        this.emailSender = emailSender;
        this.appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task Handle(GenerateTokenResetPasswordUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var code = await userManager.GeneratePasswordResetTokenAsync(user);

        var urlOfSendMail = QueryHelpers.AddQueryString($"{appSettings.FrontendUrl}/reset-password/confirm",
            new Dictionary<string, string>()
            {
                { "email", user.Email! },
                { "code", code }
            });

        await emailSender.SendEmailAsync(
            $"<div>Please reset your password by <a href='{urlOfSendMail}'>clicking here</a>.</div>",
            "Reset your password",
            new List<string> { user.Email! }, cancellationToken);
    }
}
