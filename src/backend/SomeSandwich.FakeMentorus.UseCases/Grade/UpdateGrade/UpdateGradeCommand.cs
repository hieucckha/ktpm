using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.UpdateGrade;

/// <summary>
/// Represents the command to update a grade.
/// </summary>
public class UpdateGradeCommand : IRequest
{
    /// <summary>
    /// Grade id.
    /// </summary>
    required public int GradeId { get; set; }

    /// <summary>
    /// Grade value.
    /// </summary>
    required public float GradeValue { get; set; }
}
