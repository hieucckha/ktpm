using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseIdExport;

/// <summary>
/// Handler for <see cref="GetAllGradeByCourseIdExportQuery"/>.
/// </summary>
internal class GetAllGradeByCourseIdExportQueryHandler : IRequestHandler<
    GetAllGradeByCourseIdExportQuery, GetAllGradeByCourseIdExportResult>
{
    private readonly ILogger<GetAllGradeByCourseIdExportQueryHandler> logger;
    private readonly IAppDbContext appDbContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appDbContext">Database context instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    public GetAllGradeByCourseIdExportQueryHandler(
        ILogger<GetAllGradeByCourseIdExportQueryHandler> logger, IAppDbContext appDbContext,
        IMapper mapper)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GetAllGradeByCourseIdExportResult> Handle(
        GetAllGradeByCourseIdExportQuery query,
        CancellationToken cancellationToken)
    {
        // List all user
        var studentUsers = await appDbContext.Users
            .Include(e => e.ClassesStudent)
            .Where(e => e.ClassesStudent.Any(cs => cs.CourseId == query.CourseId))
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
            .Where(e => e.CourseId == query.CourseId)
            .Where(e => !studentIds.Contains(e.StudentId))
            .ToListAsync(cancellationToken);

        var listStudentNames = await appDbContext.StudentInfos
            .Where(e => e.CourseId == query.CourseId)
            .ToDictionaryAsync(e => e.StudentId, e => e.Name, cancellationToken);

        var studentId2 = studentWithoutUserid.Where(e => !studentIds.Contains(e.StudentId))
            .Select(e => e.StudentId)
            .ToList();

        // All grade
        var gradeComposites = await appDbContext.GradeCompositions
            .Include(e => e.Grades)
            .Where(e => e.CourseId == query.CourseId && e.IsDeleted == false)
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
                        Id = null,
                        GradeCompositionId = gradeComposition.Id,
                        GradeValue = null,
                        IsRequested = false
                    });
            }

            index++;
        }

        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Grade");

        var headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("StudentId");
        for (var i = 1; i < gradeComposites.Count + 1; i++)
        {
            headerRow.CreateCell(i).SetCellValue(gradeComposites[i - 1].Name);
        }

        var rowIndex = 1;
        foreach (var (key, value) in gradeTable)
        {
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue(key);
            for (var i = 1; i < value.Count + 1; i++)
            {
                var cellValue = value[i - 1].GradeValue is null
                    ? string.Empty
                    : value[i - 1].GradeValue.ToString();
                row.CreateCell(i).SetCellValue(cellValue);
            }

            rowIndex++;
        }

        var memoryStream = new MemoryStream();
        workbook.Write(memoryStream, true);
        memoryStream.Position = 0;

        return new GetAllGradeByCourseIdExportResult
        {
            FileContent = memoryStream, FileName = $"Grade_Class_{query.CourseId}.xlsx",
        };
    }
}
