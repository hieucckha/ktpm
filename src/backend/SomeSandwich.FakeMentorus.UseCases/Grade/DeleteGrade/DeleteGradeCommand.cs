using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.DeleteGrade;

/// <summary>
/// Represents the command to delete a grade.
/// </summary>
public class DeleteGradeCommand : IRequest
{
    /// <summary>
    /// Grade id.
    /// </summary>
    public int GradeId { get; set; }
}
