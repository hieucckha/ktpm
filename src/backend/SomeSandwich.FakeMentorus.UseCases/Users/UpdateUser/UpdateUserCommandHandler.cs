using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Users.UpdateStudentId;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateUser;

/// <summary>
/// Command to update user.
/// </summary>
internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly ILogger<UpdateUserCommandHandler> logger;
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
    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, UserManager<User> userManager,
        ILoggedUserAccessor loggedUserAccessor, IMediator mediator, IAppDbContext dbContext)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.loggedUserAccessor = loggedUserAccessor;
        this.mediator = mediator;
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var loggedUserId = loggedUserAccessor.GetCurrentUserId();
        logger.LogInformation($"User {loggedUserId} is updating his profile.");

        var user = dbContext.Users.FirstOrDefault(x => x.Id == loggedUserId);

        if (user == null)
        {
            logger.LogError($"User {loggedUserId} not found.");
            throw new Exception($"User {loggedUserId} not found.");
        }

        // if (request.Email != null)
        // {
        //     await userManager.SetEmailAsync(user, request.Email);
        // }

        var role = userManager.GetRolesAsync(user).Result.FirstOrDefault();

        // user.Email = request.Email ?? user.Email;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;

        // if (await dbContext.Users.AnyAsync(x => x.Email == request.Email && x.Id != loggedUserId, cancellationToken))
        // {
        //     logger.LogError($"User with email {request.Email} already exists.");
        //     throw new ConflictException($"User with email {request.Email} already exists.");
        // }

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
        logger.LogInformation($"User {loggedUserId} {user.StudentId}updated.");
    }
}
