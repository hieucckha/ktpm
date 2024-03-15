using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.CreateUser;

/// <summary>
/// Handler for <see cref="CreateUserCommand"/>.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly ILogger<CreateUserCommandHandler> logger;
    private readonly UserManager<User> userManager;
    private readonly IEmailSender emailSender;
    private readonly IAppSettings appSettings;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="userManager"></param>
    /// <param name="emailSender"></param>
    /// <param name="appSettings"></param>
    /// <param name="appDbContext"></param>
    public CreateUserCommandHandler(
        ILogger<CreateUserCommandHandler> logger,
        UserManager<User> userManager,
        IEmailSender emailSender, IAppSettings appSettings, IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.emailSender = emailSender;
        this.appSettings = appSettings;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Creating user with email: {command.Email}.");

        var user = new User
        {
            Email = command.Email,
            UserName = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        if (!string.IsNullOrEmpty(command.StudentId))
        {
            if (!await appDbContext.Students.AnyAsync(s => s.StudentId == command.StudentId, cancellationToken))
            {
                appDbContext.Students.Add(new Student { StudentId = command.StudentId });
            }

            if (await appDbContext.Users.AnyAsync(e => e.StudentId == command.StudentId, cancellationToken))
            {
                logger.LogError($"Student with id {command.StudentId} already exists.");
                throw new DomainException($"Student with id {command.StudentId} already exists.");
            }

            user.StudentId = command.StudentId;
        }

        var result = await userManager.CreateAsync(user, command.Password);
        logger.LogInformation("User creation result: {Result}", result);
        if (result.Succeeded)
        {
            logger.LogInformation("User id: {UserId}", user.Id);
        }
        else
        {
            logger.LogError("User creation failed: {Errors}.", result.Errors);
            throw new DomainException(
                $"User creation failed: {result.Errors.FirstOrDefault()?.Description}.");
        }

        if (!string.IsNullOrEmpty(command.StudentId))
        {
            await userManager.AddToRoleAsync(user, "Student");
            logger.LogInformation($"User with id {user.Id} was created with role 'Student'.");
        }
        else
        {
            await userManager.AddToRoleAsync(user, "Teacher");
            logger.LogInformation($"User with id {user.Id} was created with role 'Teacher'.");
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var urlOfSendMail = QueryHelpers.AddQueryString($"{appSettings.FrontendUrl}/activate-account/confirm",
            new Dictionary<string, string>() { { "email", user.Email }, { "code", code } });

        await emailSender.SendEmailAsync(
            $"<div>Please confirm your account by <a href='{urlOfSendMail}'>clicking here</a>.</div>",
            "Activate your account",
            new List<string> { user.Email }, cancellationToken);

        logger.LogInformation("Send email confirm to email {email}", command.Email);
    }
}
