using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentGradeTemplate;

/// <summary>
/// Handler for <see cref="GenerateStudentGradeTemplateCommand"/>.
/// </summary>
internal class GenerateStudentGradeTemplateCommandHandler : IRequestHandler<GenerateStudentGradeTemplateCommand, GenerateStudentGradeTemplateResult>
{
    private readonly ILogger<GenerateStudentGradeTemplateCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appDbContext">Database context instance.</param>
    public GenerateStudentGradeTemplateCommandHandler(ILogger<GenerateStudentGradeTemplateCommandHandler> logger, IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task<GenerateStudentGradeTemplateResult> Handle(GenerateStudentGradeTemplateCommand command, CancellationToken cancellationToken)
    {
        var gradeCompositions = await appDbContext.GradeCompositions
            .Where(e => e.IsDeleted == false)
            .Where(e => e.CourseId == command.CourseId)
            .ToListAsync(cancellationToken);

        if (gradeCompositions.Count == 0)
        {
            throw new DomainException(
                "No grade composite in this class. Please create few grade composite and try again.");
        }

        var studentInfos = await appDbContext.StudentInfos
            .Where(e => e.CourseId == command.CourseId)
            .OrderBy(e => e.StudentId)
            .ToListAsync(cancellationToken);

        if (studentInfos.Count == 0)
        {
            throw new DomainException("No student in this class. Please invite some student and try again.");
        }

        var workbook = new XSSFWorkbook();
        var gradeSheet = workbook.CreateSheet("Grade");

        var headerRow = gradeSheet.CreateRow(0);

        var studentHeader = new[] { "Student Id" };
        for (var i = 0; i < studentHeader.Length; ++i)
        {
            headerRow.CreateCell(i).SetCellValue(studentHeader[i]);
        }

        var columnIndex = studentHeader.Length;
        foreach (var gradeComposition in gradeCompositions)
        {
            headerRow.CreateCell(columnIndex).SetCellValue(gradeComposition.Name);
            columnIndex++;
        }

        var rowIndex = 1;
        foreach (var studentInfo in studentInfos)
        {
            var row = gradeSheet.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(studentInfo.StudentId);

            rowIndex++;
        }

        var fileName = $"Class_{command.CourseId}_Student_Grade_Template.xlsx";
        var stream = new MemoryStream();
        workbook.Write(stream, true);

        logger.LogInformation("Generate student grade template for CourseId {courseId}", command.CourseId);

        return new GenerateStudentGradeTemplateResult
        {
            FileContent = stream,
            FileName = fileName
        };
    }
}
