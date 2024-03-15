using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Common.Utils;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;
using SomeSandwich.FakeMentorus.UseCases.GradeComposition.Common;
using SomeSandwich.FakeMentorus.UseCases.Users.Common.Dtos;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseId;

/// <summary>
/// Handler for <see cref="GetAllGradeByCourseIdCommand"/>.
/// </summary>
internal class
    GetAllGradeByCourseIdCommandHandler : IRequestHandler<GetAllGradeByCourseIdCommand, GetAllGradeByCourseIdResult>
{
    private readonly ILogger<GetAllGradeByCourseIdCommandHandler> logger;
    private readonly IAppDbContext appDbContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appDbContext"></param>
    /// <param name="mapper"></param>
    public GetAllGradeByCourseIdCommandHandler(ILogger<GetAllGradeByCourseIdCommandHandler> logger,
        IAppDbContext appDbContext, IMapper mapper)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GetAllGradeByCourseIdResult> Handle(GetAllGradeByCourseIdCommand command,
        CancellationToken cancellationToken)
    {
        // List all user
        var studentUsers = await appDbContext.Users
            .Include(e => e.ClassesStudent)
            .Where(e => e.ClassesStudent.Any(cs => cs.CourseId == command.CourseId))
            .ToListAsync(cancellationToken);

        var studentIds = studentUsers
            .Where(e => e.StudentId != null)
            .Select(e => e.StudentId)
            .ToList();

        // UserId | Not StudentId
        var userWithoutStudentId = studentUsers
            .Where(e => e.StudentId == null)
            .ToList();

        // UserId | StudentId
        var userWithStudentId = studentUsers
            .Where(e => e.StudentId != null)
            .ToList();

        var studentId1 = userWithStudentId.Select(e => e.StudentId).ToList();

        // Not UserId | StudentId
        var studentWithoutUserid = await appDbContext.StudentInfos
            .Where(e => e.CourseId == command.CourseId)
            .Where(e => !studentIds.Contains(e.StudentId))
            .ToListAsync(cancellationToken);

        var listStudentNames = await appDbContext.StudentInfos
            .Where(e => e.CourseId == command.CourseId)
            .ToDictionaryAsync(e => e.StudentId, e => e.Name, cancellationToken);

        var studentId2 = studentWithoutUserid.Where(e => !studentIds.Contains(e.StudentId)).Select(e => e.StudentId)
            .ToList();

        // All grade
        var gradeComposites = await appDbContext.GradeCompositions
            .Include(e => e.Grades)
            .Where(e => e.CourseId == command.CourseId && e.IsDeleted == false)
            .OrderBy(e => e.GradeScale)
            .ThenBy(e => e.Id)
            .ToListAsync(cancellationToken);

        var gradeTable = new Dictionary<string, List<GradeCellDto>>();
        foreach (var studentId in studentIds)
        {
            gradeTable.Add(studentId, new List<GradeCellDto>(gradeComposites.Count));
        }

        foreach (var studentId in studentId2)
        {
            gradeTable.Add(studentId, new List<GradeCellDto>(gradeComposites.Count));
        }

        var index = 0;
        foreach (var gradeComposition in gradeComposites)
        {
            foreach (var grade in gradeComposition.Grades)
            {
                var gradeStudent = gradeTable.GetValueOrDefault(grade.StudentId);

                gradeStudent?.Insert(index, mapper.Map<GradeCellDto>(grade));
            }

            foreach (var (key, value) in gradeTable.Where(pair => pair.Value.Count != index + 1))
            {
                value.Insert(index,
                    new GradeCellDto
                    {
                        Id = null, GradeCompositionId = gradeComposition.Id, GradeValue = null, IsRequested = false
                    });
            }

            index++;
        }

        var gradeUserAndStudent = gradeTable
            .Where(e => studentId1.Contains(e.Key))
            .Select(pair =>
            {
                var student = userWithStudentId.First(e => e.StudentId == pair.Key);
                var studentName = listStudentNames.GetValueOrDefault(pair.Key);

                return new GradeCell
                {
                    StudentId = pair.Key,
                    StudentName = studentName ?? student.FullName,
                    UserId = student.Id,
                    GradeDto = pair.Value
                };
            }).ToList();

        var gradeNotUserAndStudent = gradeTable
            .Where(e => studentId2.Contains(e.Key))
            .Select(pair => new GradeCell
            {
                StudentId = pair.Key,
                StudentName =
                    studentWithoutUserid.First(e => e.StudentId == pair.Key).Name,
                UserId = null,
                GradeDto = pair.Value
            }).ToList();

        var students = gradeUserAndStudent.Concat(gradeNotUserAndStudent).OrderBy(e => e.StudentId).ToList();

        var result = new GetAllGradeByCourseIdResult
        {
            CourseId = command.CourseId,
            GradeCompositionDtos = mapper.Map<IReadOnlyList<GradeCompositionDto>>(gradeComposites),
            Students = students,
            UserWithoutStudentId = userWithoutStudentId.Select(e => mapper.Map<UserWithoutStudentDto>(e)).ToList()
        };

        return result;
    }
}
