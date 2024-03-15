using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.ImportAllStudentList;

/// <summary>
/// Command for import all student list.
/// </summary>
public class ImportAllStudentListCommand : IRequest
{
    /// <summary>
    /// Student grade template file.
    /// </summary>
    [Required]
    required public Stream FileContent { get; set; }
}
