using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentGrade;

/// <summary>
/// Handler for <see cref="ImportStudentGradeCommand"/>.
/// </summary>
internal class ImportStudentGradeCommandHandler : IRequestHandler<ImportStudentGradeCommand>
{
    private readonly ILogger<ImportStudentGradeCommandHandler> logger;
    private readonly IAppDbContext appDbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="appDbContext">Database context instance.</param>
    public ImportStudentGradeCommandHandler(ILogger<ImportStudentGradeCommandHandler> logger,
        IAppDbContext appDbContext)
    {
        this.logger = logger;
        this.appDbContext = appDbContext;
    }

    /// <inheritdoc />
    public async Task Handle(ImportStudentGradeCommand command, CancellationToken cancellationToken)
    {
        var workbook = new XSSFWorkbook(command.FileContent);
        var gradeSheet = workbook.GetSheetAt(0);

        if (gradeSheet is null)
        {
            throw new NotFoundException("The file have not any sheet. Please try again");
        }

        var headerRow = gradeSheet.GetRow(0);
        if (headerRow is null || headerRow.Cells.Count() < 2)
        {
            throw new DomainException("The file is empty or not have any grade composition.");
        }

        // Student Id | Grade Composition 1 | Grade Composition 2 | Grade Composition 3 | ...
        var gradeComposites = new List<Domain.Grade.GradeComposition>();
        var gradeIndex = new List<int>();

        var headerIndex = 0;
        foreach (var cell in headerRow.Cells)
        {
            if (headerIndex == 0)
            {
                headerIndex++;
                continue;
            }

            var gradeComposition = await appDbContext.GradeCompositions
                .FirstOrDefaultAsync(e => e.Name == cell.StringCellValue, cancellationToken);

            if (gradeComposition is null)
            {
                headerIndex++;
                continue;
            }

            gradeComposites.Add(gradeComposition);
            gradeIndex.Add(headerIndex);
            headerIndex++;
        }

        var rowIndex = 1;
        var row = gradeSheet.GetRow(rowIndex);
        while (row is not null)
        {
            var studentId = row.GetCell(0).StringCellValue;
            if (studentId is not null)
            {
                for (var i = 0; i < gradeIndex.Count; ++i)
                {
                    var column = gradeIndex[i];

                    var cell = row.GetCell(column);
                    if (cell is null)
                    {
                        continue;
                    }

                    var gradeValue = cell.CellType == CellType.Numeric
                        ? cell.NumericCellValue
                        : double.TryParse(cell.StringCellValue, out var gradeValueParse)
                            ? gradeValueParse
                            : 0;

                    var grade = await appDbContext.Grades
                        .Where(e => e.StudentId == studentId)
                        .FirstOrDefaultAsync(e => e.GradeCompositionId == gradeComposites[i].Id,
                            cancellationToken);

                    if (grade is null)
                    {
                        await appDbContext.Grades.AddAsync(
                            new Domain.Grade.Grade
                            {
                                GradeCompositionId = gradeComposites[i].Id,
                                StudentId = studentId,
                                GradeValue = (float)gradeValue,
                            }, cancellationToken);
                    }
                    else
                    {
                        grade.GradeValue = (float)gradeValue;
                        grade.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }


            rowIndex++;
            row = gradeSheet.GetRow(rowIndex);
        }

        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
