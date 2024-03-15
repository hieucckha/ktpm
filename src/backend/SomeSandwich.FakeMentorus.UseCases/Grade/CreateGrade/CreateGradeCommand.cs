using System.ComponentModel.DataAnnotations;
using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.CreateGrade;

/// <summary>
/// Command to create a new grade
/// </summary>
public class CreateGradeCommand : IRequest<GradeDto>
{
    /// <summary>
    ///     Grade Composition id.
    /// </summary>
    required public int GradeCompositionId { get; set; }

    /// <summary>
    ///     Student id.
    /// </summary>
    required public string StudentId { get; set; }

    /// <summary>
    ///     Grade value.
    /// </summary>
    [Range(0, 10)]
    required public float GradeValue { get; set; }
}
