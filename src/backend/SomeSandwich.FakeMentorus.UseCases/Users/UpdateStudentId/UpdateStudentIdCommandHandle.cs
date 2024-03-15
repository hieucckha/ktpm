using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateStudentId;

/// <summary>
/// Command to update student id.
/// </summary>
public class UpdateStudentIdCommandHandle : IRequestHandler<UpdateStudentIdCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStudentIdCommandHandle"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public UpdateStudentIdCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateStudentIdCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.GetAsync(x => x.Id == command.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User not found");
        }

        if (command.StudentId == null)
        {
            user.StudentId = null;
        }
        else
        {
            var student =
                await dbContext.Students.FirstOrDefaultAsync(x => x.StudentId == command.StudentId, cancellationToken);
            if (student == null)
            {
                await dbContext.Students.AddAsync(new Student { StudentId = command.StudentId }, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            user.StudentId = command.StudentId;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
