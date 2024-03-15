using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.LockoutUser.UnlockUser;

/// <summary>
/// Handler for <see cref="UnlockUserCommand"/>.
/// </summary>
internal class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand>
{
    private readonly ILogger<UnlockUserCommandHandler> logger;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    public UnlockUserCommandHandler(ILogger<UnlockUserCommandHandler> logger,
        UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(UnlockUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            throw new NotFoundException($"User with id {command.UserId} not found.");
        }

        if (user.LockoutEnabled == false)
        {
            throw new DomainException(
                $"User with id {command.UserId} is not lock. Can not unlock a user not lock.");
        }

        await userManager.SetLockoutEnabledAsync(user, false);
        await userManager.SetLockoutEndDateAsync(user, null);

        logger.LogInformation("User with id {UserId} is unlock.", command.UserId);
    }
}
