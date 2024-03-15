using MediatR;
using Microsoft.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.CreateGradeComposition;

/// <summary>
/// Handle for <see cref="CreateGradeCompositionCommand"/>.
/// </summary>
internal class CreateGradeCompositionCommandHandle : IRequestHandler<CreateGradeCompositionCommand, int>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    public CreateGradeCompositionCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<int> Handle(CreateGradeCompositionCommand request, CancellationToken cancellationToken)
    {
        var gradeComposition = new Domain.Grade.GradeComposition
        {
            Name = request.Name,
            CourseId = request.CourseId,
            Description = request.Description,
            GradeScale = request.GradeScale
        };
        var listGradeComposition = await dbContext.GradeCompositions
            .Where(e => e.CourseId == request.CourseId)
            .OrderByDescending(e => e.Order)
            .ToListAsync(cancellationToken: cancellationToken);

        if (listGradeComposition.Count > 0)
        {
            gradeComposition.Order = listGradeComposition[0].Order + 1;
        }
        else
        {
            gradeComposition.Order = 1;
        }


        await dbContext.GradeCompositions.AddAsync(gradeComposition, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return gradeComposition.Id;
    }
}
