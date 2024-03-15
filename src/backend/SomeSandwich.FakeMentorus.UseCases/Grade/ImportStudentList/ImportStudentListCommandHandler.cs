using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentList;

/// <summary>
/// Handler for <see cref="ImportStudentListCommand"/>.
/// </summary>
internal class ImportStudentListCommandHandler : IRequestHandler<ImportStudentListCommand>
{
    private readonly ILogger<ImportStudentListCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="appDbContext"></param>
    public ImportStudentListCommandHandler(ILogger<ImportStudentListCommandHandler> logger, IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task Handle(ImportStudentListCommand command, CancellationToken cancellationToken)
    {
        using var workbook = new XSSFWorkbook(command.FileContent);
        var sheet = workbook.GetSheetAt(0);

        if (sheet is null)
        {
            throw new DomainException("The workbook is empty. Please try again");
        }

        // Header order: Student Id, Student Name
        var rowIndex = 1;
        var row = sheet.GetRow(rowIndex);
        if (row is null)
        {
            return;
        }

        var studentId = string.Empty;

        var studentIdCell = row.GetCell(0);
        studentId = studentIdCell.CellType == CellType.Numeric
            ? studentIdCell.NumericCellValue.ToString(CultureInfo.CurrentCulture)
            : studentIdCell.StringCellValue;
        var studentNameCell = row.GetCell(1);
        var studentName = studentNameCell.StringCellValue;

        while (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(studentName))
        {
            var student =
                await appDbContext.Students.FirstOrDefaultAsync(e => e.StudentId == studentId, cancellationToken);
            if (student is null)
            {
                student = new Student { StudentId = studentId };
                await appDbContext.Students.AddAsync(student, cancellationToken);

                logger.LogInformation("Create student with id {studentId}", studentId);
            }

            var studentInfo = await appDbContext.StudentInfos
                .Where(e => e.CourseId == command.CourseId)
                .FirstOrDefaultAsync(e => e.StudentId == studentId, cancellationToken);

            if (studentInfo is null)
            {
                studentInfo = new StudentInfo
                {
                    StudentId = studentId, Student = student, Name = studentName, CourseId = command.CourseId
                };
                await appDbContext.StudentInfos.AddAsync(studentInfo, cancellationToken);

                logger.LogInformation(
                    "Create student information in course id {courseId} with id {studentId}, name {studentName}",
                    command.CourseId, studentId, studentName);
            }

            rowIndex++;
            row = sheet.GetRow(rowIndex);

            if (row is null)
            {
                break;
            }

            studentIdCell = row.GetCell(0);
            studentId = studentIdCell.CellType == CellType.Numeric
                ? studentIdCell.NumericCellValue.ToString(CultureInfo.CurrentCulture)
                : studentIdCell.StringCellValue;
            studentNameCell = row.GetCell(1);
            studentName = studentNameCell.StringCellValue;
        }

        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
