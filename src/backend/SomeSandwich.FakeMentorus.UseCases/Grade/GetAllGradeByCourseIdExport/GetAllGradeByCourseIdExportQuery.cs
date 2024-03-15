using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseIdExport;

/// <summary>
/// Query to get all grade by course id.
/// </summary>
public class GetAllGradeByCourseIdExportQuery : IRequest<GetAllGradeByCourseIdExportResult>
{
    /// <summary>
    /// Course id.
    /// </summary>
    [Required]
    required public int CourseId { get; set; }
}
