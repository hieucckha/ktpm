using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ImportStudentIdTemplate;

/// <summary>
/// Import student id from template command.
/// </summary>
public class ImportStudentIdTemplateCommand : IRequest
{
    /// <summary>
    /// Student grade template file.
    /// </summary>
    [Required]
    required public Stream FileContent { get; set; }
}
