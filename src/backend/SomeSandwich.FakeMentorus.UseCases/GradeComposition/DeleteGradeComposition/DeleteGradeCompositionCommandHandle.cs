using MediatR;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.DeleteGradeComposition;

internal class DeleteGradeCompositionCommandHandle : IRequestHandler<DeleteGradeCompositionCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteGradeCompositionCommandHandle"/> class.
    /// </summary>
    /// <param name="dbContext">Database instance.</param>
    public DeleteGradeCompositionCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(DeleteGradeCompositionCommand request, CancellationToken cancellationToken)
    {
        var gradeComposition = await dbContext.GradeCompositions
            .GetAsync(x => x.Id == request.GradeCompositionId, cancellationToken);

        if (gradeComposition is null)
        {
            throw new NotFoundException($"Grade composition not found.");
        }

        gradeComposition.IsDeleted = true;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
