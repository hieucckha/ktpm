using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.XSSF.UserModel;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentListAllTemplate;

/// <summary>
/// Handler for <see cref="GenerateStudentListAllTemplateCommandHandler"/>.
/// </summary>
public class GenerateStudentListAllTemplateCommandHandler : IRequestHandler<GenerateStudentListAllTemplateCommand,
    GenerateStudentListAllTemplateResult>
{
    private readonly ILogger<GenerateStudentListAllTemplateCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="appDbContext"></param>
    /// <param name="logger"></param>
    public GenerateStudentListAllTemplateCommandHandler(IAppDbContext appDbContext,
        ILogger<GenerateStudentListAllTemplateCommandHandler> logger)
    {
        this.appDbContext = appDbContext;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<GenerateStudentListAllTemplateResult> Handle(GenerateStudentListAllTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var students = await appDbContext.Users
            .ToListAsync(cancellationToken);

        var workbook = new XSSFWorkbook();
        var gradeSheet = workbook.CreateSheet("Students");

        var headerRow = gradeSheet.CreateRow(0);

        var studentHeader = new[] { "Email", "StudentId" };
        for (var i = 0; i < studentHeader.Length; ++i)
        {
            headerRow.CreateCell(i).SetCellValue(studentHeader[i]);
        }

        var rowIndex = 1;
        foreach (var student in students)
        {
            var row = gradeSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(student.Email);
            row.CreateCell(1).SetCellValue(student.StudentId);
        }

        var fileName = $"Student_List.xlsx";
        var stream = new MemoryStream();
        workbook.Write(stream, true);

        logger.LogInformation("Generate student list template for all");

        return new GenerateStudentListAllTemplateResult { FileContent = stream, FileName = fileName };
    }
}
