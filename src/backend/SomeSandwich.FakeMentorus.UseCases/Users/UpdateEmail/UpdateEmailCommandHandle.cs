using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Users.UpdateUser;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateEmail;

/// <summary>
/// Handler for <see cref="UpdateEmailCommand" />.
/// </summary>
public class UpdateEmailCommandHandle : IRequestHandler<UpdateEmailCommand>
{
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly ILogger<UpdateEmailCommandHandle> logger;
    private readonly UserManager<User> userManager;
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    /// <param name="dbContext"></param>
    public UpdateEmailCommandHandle(ILoggedUserAccessor loggedUserAccessor, ILogger<UpdateEmailCommandHandle> logger,
        UserManager<User> userManager, IAppDbContext dbContext)
    {
        this.loggedUserAccessor = loggedUserAccessor;
        this.logger = logger;
        this.userManager = userManager;
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        request.UserId ??= loggedUserAccessor.GetCurrentUserId();

        var user = dbContext.Users.FirstOrDefault(x => x.Id == request.UserId);
        if (user == null)
        {
            logger.LogError($"User {request.UserId} not found.");
            throw new Exception($"User {request.UserId} not found.");
        }

        if (await dbContext.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.UserId, cancellationToken))
        {
            logger.LogError($"User with email {request.Email} already exists.");
            throw new Exception($"User with email {request.Email} already exists.");
        }

        await userManager.SetEmailAsync(user, request.Email);
        await userManager.SetUserNameAsync(user, request.Email);
    }
}
