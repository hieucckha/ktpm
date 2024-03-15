using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.LockoutUser.LockUser;

/// <summary>
/// Handler for <see cref="LockUserCommand"/>.
/// </summary>
internal class LockUserCommandHandler : IRequestHandler<LockUserCommand>
{
    private readonly ILogger<LockUserCommandHandler> logger;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="userManager">User manager instance.</param>
    public LockUserCommandHandler(ILogger<LockUserCommandHandler> logger,
        UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(LockUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            throw new NotFoundException($"User with id {command.UserId} not found.");
        }

        if (user.LockoutEnabled)
        {
            throw new DomainException($"User with id {command.UserId} is lock. Can not lock again");
        }

        await userManager.SetLockoutEnabledAsync(user, true);
        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

        logger.LogInformation("User with id {UserId} is lock.", command.UserId);
    }
}
