using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentListTemplate;

/// <summary>
/// Command to generate student list template.
/// </summary>
public class GenerateStudentListTemplateCommand : IRequest<GenerateStudentListTemplateResult>
{
    /// <summary>
    /// Id of course which will to generate template.
    /// </summary>
    [Required]
    required public int CourseId { get; set; }
}
