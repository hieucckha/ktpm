using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ActivateUser.RegisterActivateUser;

/// <summary>
/// Handler for <see cref="RegisterActivateUserCommand"/>.
/// </summary>
public class RegisterActivateUserCommandHandler : IRequestHandler<RegisterActivateUserCommand>
{
    private readonly IAppSettings appSettings;
    private readonly UserManager<User> userManager;
    private readonly IEmailSender emailSender;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="emailSender">Email sender service.</param>
    /// <param name="appSettings"></param>
    public RegisterActivateUserCommandHandler(UserManager<User> userManager, IEmailSender emailSender, IAppSettings appSettings)
    {
        this.userManager = userManager;
        this.emailSender = emailSender;
        this.appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task Handle(RegisterActivateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        if (user.EmailConfirmed == false)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var urlOfSendMail = QueryHelpers.AddQueryString($"{appSettings.FrontendUrl}/activate-account/confirm",
                new Dictionary<string, string>()
                {
                    { "email", user.Email! },
                    { "code", code }
                });

            await emailSender.SendEmailAsync(
                $"<div>Please confirm your account by <a href='{urlOfSendMail}'>clicking here</a>.</div>",
                "Activate your account",
                new List<string> { user.Email! }, cancellationToken);
        }
    }
}
