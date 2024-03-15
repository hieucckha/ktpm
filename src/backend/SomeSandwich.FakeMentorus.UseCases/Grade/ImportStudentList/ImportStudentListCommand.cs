using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentList;

/// <summary>
/// Command for importing a student list for a specific course.
/// </summary>
public class ImportStudentListCommand : IRequest
{
    /// <summary>
    /// Id of course.
    /// </summary>
    [Required]
    required public int CourseId { get; set; }

    /// <summary>
    /// Student grade template file.
    /// </summary>
    [Required]
    required public Stream FileContent { get; set; }
}
