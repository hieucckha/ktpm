using MediatR;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.DeleteGrade;

/// <summary>
/// Handle for <see cref="DeleteGradeCommand"/>.
/// </summary>
internal class DeleteGradeCommandHandle : IRequestHandler<DeleteGradeCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    public DeleteGradeCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await dbContext.Grades.GetAsync(x => x.Id == request.GradeId, cancellationToken: cancellationToken);

        if (grade == null)
        {
            throw new NotFoundException("Grade not found");
        }

        dbContext.Grades.Remove(grade);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
