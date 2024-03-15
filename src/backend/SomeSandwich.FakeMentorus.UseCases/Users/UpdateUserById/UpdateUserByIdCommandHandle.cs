using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Users.UpdateUser;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateUserById;

/// <summary>
/// Handler for <see cref="UpdateUserByIdCommand" />.
/// </summary>
public class UpdateUserByIdCommandHandle : IRequestHandler<UpdateUserByIdCommand>
{
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly ILogger<UpdateUserByIdCommandHandle> logger;
    private readonly UserManager<User> userManager;
    private readonly IMediator mediator;
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="mediator"></param>
    /// <param name="dbContext"></param>
    public UpdateUserByIdCommandHandle(ILogger<UpdateUserByIdCommandHandle> logger, UserManager<User> userManager,
        ILoggedUserAccessor loggedUserAccessor, IMediator mediator, IAppDbContext dbContext)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.loggedUserAccessor = loggedUserAccessor;
        this.mediator = mediator;
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        var loggedUserId = loggedUserAccessor.GetCurrentUserId();
        // logger.LogInformation($"User {loggedUserId} is updating his profile.");

        var user = dbContext.Users.FirstOrDefault(x => x.Id == request.UserId);

        if (user == null)
        {
            logger.LogError($"User {request.UserId} not found.");
            throw new Exception($"User {request.UserId} not found.");
        }

        var role = userManager.GetRolesAsync(user).Result.FirstOrDefault();

        // if (await dbContext.Users.AnyAsync(x => x.Email == request.Email && x.Id != request.UserId, cancellationToken))
        // {
        //     logger.LogError($"User with email {request.Email} already exists.");
        //     throw new ConflictException($"User with email {request.Email} already exists.");
        // }

        // user.Email = request.Email ?? user.Email;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;

        if (role == "Student")
        {
            if (request.StudentId == null)
            {
                user.StudentId = null;
            }
            else
            {
                var student =
                    await dbContext.Students.FirstOrDefaultAsync(x => x.StudentId == request.StudentId,
                        cancellationToken);
                if (student == null)
                {
                    await dbContext.Students.AddAsync(new Student { StudentId = request.StudentId },
                        cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

                user.StudentId = request.StudentId;
            }
        }

        user.UpdatedAt = DateTime.Now;

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation($"User {loggedUserId} {user.StudentId} updated.");
    }
}
