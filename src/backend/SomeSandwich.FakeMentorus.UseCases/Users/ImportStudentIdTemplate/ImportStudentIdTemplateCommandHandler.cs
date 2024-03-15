using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ImportStudentIdTemplate;

/// <summary>
/// Handler for <see cref="ImportStudentIdTemplateCommand"/>.
/// </summary>
internal class ImportStudentIdTemplateCommandHandler : IRequestHandler<ImportStudentIdTemplateCommand>
{
    private readonly ILogger<ImportStudentIdTemplateCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appDbContext">Database context instance.</param>
    public ImportStudentIdTemplateCommandHandler(ILogger<ImportStudentIdTemplateCommandHandler> logger, IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task Handle(ImportStudentIdTemplateCommand command, CancellationToken cancellationToken)
    {
        var workbook = new XSSFWorkbook(command.FileContent);
        var sheet = workbook.GetSheetAt(0);

        if (sheet is null)
        {
            throw new DomainException("Cannot find the sheet in excel");
        }

        // Header order: Email, StudentId
        var rowIndex = 1;
        var row = sheet.GetRow(rowIndex);
        if (row is null)
        {
            return;
        }

        var studentEmailCell = row.GetCell(0);
        var studentEmail = studentEmailCell.StringCellValue;
        var studentIdCell = row.GetCell(1);
        var studentId = studentIdCell.CellType == CellType.Numeric
            ? studentIdCell.NumericCellValue.ToString(CultureInfo.InvariantCulture) : studentIdCell.StringCellValue;

        while (!string.IsNullOrEmpty(studentEmail) || !string.IsNullOrEmpty(studentId))
        {
            if (string.IsNullOrEmpty(studentEmail) && !string.IsNullOrEmpty(studentId))
            {
                break;
            }
            var user = await appDbContext.Users.FirstOrDefaultAsync(e => e.Email == studentEmail, cancellationToken);
            if (user is null)
            {
                rowIndex++;
                row = sheet.GetRow(rowIndex);
                if (row is null)
                {
                    return;
                }

                studentEmailCell = row.GetCell(0);
                studentEmail = studentEmailCell.StringCellValue;
                studentIdCell = row.GetCell(1);
                studentId = studentIdCell.CellType == CellType.Numeric
                    ? studentIdCell.NumericCellValue.ToString(CultureInfo.InvariantCulture) : studentIdCell.StringCellValue;

                continue;
            }
            user.StudentId = studentId;
            await appDbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("The user with email {email} is change student id to {studentId}", studentEmail, studentId);

            rowIndex++;
            row = sheet.GetRow(rowIndex);
            if (row is null)
            {
                return;
            }

            studentEmailCell = row.GetCell(0);
            studentEmail = studentEmailCell.StringCellValue;
            studentIdCell = row.GetCell(1);
            studentId = studentIdCell.CellType == CellType.Numeric
                ? studentIdCell.NumericCellValue.ToString(CultureInfo.InvariantCulture) : studentIdCell.StringCellValue;
        }
    }
}
