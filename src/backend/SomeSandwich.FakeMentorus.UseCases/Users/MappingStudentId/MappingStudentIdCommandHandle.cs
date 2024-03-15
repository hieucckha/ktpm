using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.MappingStudentId;

/// <summary>
/// Handle for <see cref="MappingStudentIdCommand"/>.
/// </summary>
public class MappingStudentIdCommandHandle : IRequestHandler<MappingStudentIdCommand>
{
    private readonly IAppDbContext dbContext;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userManager"></param>
    public MappingStudentIdCommandHandle(IAppDbContext dbContext, UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(MappingStudentIdCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is not "Student")
        {
            throw new DomainException("User is not student");
        }

        if (command.StudentId != null &&
            await dbContext.Users.AnyAsync(e => e.StudentId == command.StudentId,
                cancellationToken))
        {
            throw new ConflictException("Student id already exists");
        }

        if (command.StudentId != null &&
            !await dbContext.Students.AnyAsync(e => e.StudentId == command.StudentId,
                cancellationToken))
        {
            await dbContext.Students.AddAsync(new Student() { StudentId = command.StudentId },
                cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        user.StudentId = command.StudentId;
        user.UpdatedAt = DateTime.Now;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
