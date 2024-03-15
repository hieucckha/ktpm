using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportAllStudentList;

/// <summary>
///
/// </summary>
public class ImportAllStudentListCommandHandler : IRequestHandler<ImportAllStudentListCommand>
{
    private readonly ILogger<ImportAllStudentListCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    ///
    /// </summary>
    /// <param name="appDbContext"></param>
    /// <param name="logger"></param>
    public ImportAllStudentListCommandHandler(IAppDbContext appDbContext,
        ILogger<ImportAllStudentListCommandHandler> logger)
    {
        this.appDbContext = appDbContext;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(ImportAllStudentListCommand command, CancellationToken cancellationToken)
    {
        using var workbook = new XSSFWorkbook(command.FileContent);
        var sheet = workbook.GetSheetAt(0);

        if (sheet is null)
        {
            throw new DomainException("The workbook is empty. Please try again");
        }

        var studentIdDict = new Dictionary<string, string>();

        // Header order: Email, StudentId
        var rowIndex = 1;
        var row = sheet.GetRow(rowIndex);
        if (row is null)
        {
            return;
        }

        var emailCell = row.GetCell(0);
        var email = emailCell.StringCellValue;

        var studentIdCell = row.GetCell(1);
        var studentId = studentIdCell.CellType == CellType.Numeric
            ? studentIdCell.NumericCellValue.ToString(CultureInfo.CurrentCulture)
            : studentIdCell.StringCellValue;

        while (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(studentId))
        {
            if (string.IsNullOrEmpty(email))
            {
                continue;
            }

            if (!string.IsNullOrEmpty(studentId))
            {
                studentIdDict.TryAdd(studentId, email);
            }

            rowIndex++;
            row = sheet.GetRow(rowIndex);

            if (row is null)
            {
                break;
            }

            emailCell = row.GetCell(0);
            if (emailCell is null)
            {
                continue;
            }

            email = emailCell.StringCellValue;

            studentIdCell = row.GetCell(1);
            if (studentIdCell is null)
            {
                continue;
            }

            studentId = studentIdCell.CellType == CellType.Numeric
                ? studentIdCell.NumericCellValue.ToString(CultureInfo.CurrentCulture)
                : studentIdCell.StringCellValue;
        }

        // Reverse to [Email, StudentId]
        studentIdDict = studentIdDict.ToDictionary(e => e.Value, e => e.Key);

        var students = await appDbContext.Users
            .ToListAsync(cancellationToken);

        foreach (var pair in studentIdDict)
        {
            var student = students.FirstOrDefault(e => e.Email == pair.Key);
            if (student is null)
            {
                continue;
            }

            if (student.StudentId == pair.Value)
            {
                continue;
            }

            var studentIdEntity =
                await appDbContext.Students.FirstOrDefaultAsync(e => e.StudentId == pair.Value, cancellationToken);

            if (studentIdEntity is null)
            {
                studentIdEntity = new Student { StudentId = pair.Value, };
            }

            student.Student = studentIdEntity;
        }

        await appDbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Import all student list");
    }
}
