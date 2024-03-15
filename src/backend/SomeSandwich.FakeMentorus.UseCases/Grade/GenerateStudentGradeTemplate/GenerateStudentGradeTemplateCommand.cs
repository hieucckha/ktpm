using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentGradeTemplate;

/// <summary>
/// Command for generate the student grade template for import grade.
/// </summary>
public class GenerateStudentGradeTemplateCommand : IRequest<GenerateStudentGradeTemplateResult>
{
    /// <summary>
    /// Id of course which will to generate template.
    /// </summary>
    [Required]
    required public int CourseId { get; set; }
}
