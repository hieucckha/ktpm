using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentGradeTemplate;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentListTemplate;

/// <summary>
/// Handler for <see cref="GenerateStudentListTemplateCommand"/>.
/// </summary>
internal class GenerateStudentListTemplateCommandHandler : IRequestHandler<GenerateStudentListTemplateCommand, GenerateStudentListTemplateResult>
{
    private readonly ILogger<GenerateStudentListTemplateCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appDbContext">Database context instance.</param>
    public GenerateStudentListTemplateCommandHandler(ILogger<GenerateStudentListTemplateCommandHandler> logger, IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task<GenerateStudentListTemplateResult> Handle(GenerateStudentListTemplateCommand command, CancellationToken cancellationToken)
    {
        var studentInfos = await appDbContext.StudentInfos
            .Where(e => e.CourseId == command.CourseId)
            .OrderBy(e => e.StudentId)
            .ToListAsync(cancellationToken);

        var workbook = new XSSFWorkbook();
        var gradeSheet = workbook.CreateSheet("Students");

        var headerRow = gradeSheet.CreateRow(0);

        var studentHeader = new[] { "Student Id", "Student Name" };
        for (var i = 0; i < studentHeader.Length; ++i)
        {
            headerRow.CreateCell(i).SetCellValue(studentHeader[i]);
        }

        var rowIndex = 1;
        foreach (var studentInfo in studentInfos)
        {
            var row = gradeSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(studentInfo.StudentId);
            row.CreateCell(1).SetCellValue(studentInfo.Name);
        }

        var fileName = $"Student_Template_Class_{command.CourseId}.xlsx";
        var stream = new MemoryStream();
        workbook.Write(stream, true);

        logger.LogInformation("Generate student list template for CourseId {courseId}", command.CourseId);

        return new GenerateStudentListTemplateResult
        {
            FileContent = stream,
            FileName = fileName
        };
    }
}
