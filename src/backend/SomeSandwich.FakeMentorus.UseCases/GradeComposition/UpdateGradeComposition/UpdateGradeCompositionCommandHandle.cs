using MediatR;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.UpdateGradeComposition;

/// <inheritdoc />
internal class UpdateGradeCompositionCommandHandle : IRequestHandler<UpdateGradeCompositionCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    public UpdateGradeCompositionCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(UpdateGradeCompositionCommand request, CancellationToken cancellationToken)
    {
        var gradeComposition = await dbContext.GradeCompositions
            .GetAsync(x => x.Id == request.GradeCompositionId && x.IsDeleted != true, cancellationToken);

        if (gradeComposition == null)
        {
            throw new NotFoundException("Grade composition not found");
        }

        var isUpdated = false;
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            isUpdated = true;
            gradeComposition.Name = request.Name;
        }

        if (request.Description is not null && gradeComposition.Description != request.Description)
        {
            isUpdated = true;
            gradeComposition.Description = request.Description;
        }

        if (request.GradeScale is not null && gradeComposition.GradeScale != request.GradeScale)
        {
            isUpdated = true;
            gradeComposition.GradeScale = (int)request.GradeScale;
        }

        if (isUpdated)
        {
            gradeComposition.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
