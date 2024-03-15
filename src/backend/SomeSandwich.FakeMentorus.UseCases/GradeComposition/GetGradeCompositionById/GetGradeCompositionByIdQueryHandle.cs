using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.GetGradeCompositionById;

/// <summary>
/// Handler for <see cref="GetGradeCompositionByIdQuery"/>.
/// </summary>
internal class
    GetGradeCompositionByIdQueryHandle : IRequestHandler<GetGradeCompositionByIdQuery, GradeCompositionDetailDto>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    public GetGradeCompositionByIdQueryHandle(IAppDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <summary>
    /// Handle request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GradeCompositionDetailDto> Handle(GetGradeCompositionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var gradeComposition = await dbContext.GradeCompositions
            .Include(e => e.Grades)
            .ThenInclude(f => f.Student)
            .GetAsync(x => x.Id == request.Id && x.IsDeleted != true, cancellationToken);

        if (gradeComposition == null)
        {
            throw new NotFoundException("Grade composition not found.");
        }

        return mapper.Map<GradeCompositionDetailDto>(gradeComposition);
    }
}
