using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.UpdateGrade;

internal class UpdateGradeCommandHandle : IRequestHandler<UpdateGradeCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    public UpdateGradeCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await dbContext.Grades.GetAsync(e => e.Id == request.GradeId, cancellationToken: cancellationToken);
        if (grade == null)
        {
            throw new NotFoundException("Grade not found");
        }

        grade.GradeValue = request.GradeValue;
        grade.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
