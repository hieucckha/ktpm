using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.CreateGrade;

/// <summary>
/// Handle for <see cref="CreateGradeCommand"/>.
/// </summary>
internal class CreateGradeCommandHandle : IRequestHandler<CreateGradeCommand, GradeDto>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    public CreateGradeCommandHandle(IAppDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GradeDto> Handle(CreateGradeCommand request, CancellationToken cancellationToken)
    {
        var student = await dbContext.StudentInfos
            .Include(si => si.Course)
            .ThenInclude(c => c.GradeCompositions)
            .Where(si => si.StudentId == request.StudentId)
            .Where(si => si.Course.GradeCompositions.Any(gc => gc.Id == request.GradeCompositionId))
            .FirstOrDefaultAsync(cancellationToken);

        var gradeOld = await dbContext.Grades.FirstOrDefaultAsync(
            e => e.GradeCompositionId == request.GradeCompositionId && e.StudentId == request.StudentId,
            cancellationToken);
        if (gradeOld != null)
        {
            // throw new DomainException("Grade already exists");
            gradeOld.GradeValue = request.GradeValue;
            gradeOld.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
            var result1 = mapper.Map<GradeDto>(gradeOld);
            result1.StudentName = student?.Name ?? "";

            return result1;
        }

        var grade = new Domain.Grade.Grade
        {
            GradeCompositionId = request.GradeCompositionId,
            StudentId = request.StudentId,
            GradeValue = request.GradeValue
        };

        await dbContext.Grades.AddAsync(grade, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<GradeDto>(grade);

        result.StudentName = student?.Name ?? "";


        return result;
    }
}
