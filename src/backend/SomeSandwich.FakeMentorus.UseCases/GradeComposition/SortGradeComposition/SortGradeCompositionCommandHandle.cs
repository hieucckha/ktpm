using MediatR;
using Microsoft.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.SortGradeComposition;

/// <summary>
/// Command to sort grade composition.
/// </summary>
public class SortGradeCompositionCommandHandle : IRequestHandler<SortGradeCompositionCommand>
{
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    public SortGradeCompositionCommandHandle(IAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(SortGradeCompositionCommand request, CancellationToken cancellationToken)
    {
        var listGradeType = await dbContext.GradeCompositions.ToListAsync(cancellationToken);

        foreach (var composition in request.GradeCompositions)
        {
            var gradeComposition = listGradeType.FirstOrDefault(x => x.Id == composition.Id);
            if (gradeComposition != null)
            {
                gradeComposition.Order = composition.Order;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
