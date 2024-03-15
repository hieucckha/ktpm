using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.ToggleIsFinished;

/// <summary>
/// Toggle is finished command.
/// </summary>
public class ToggleIsFinishedCommand : IRequest
{
    /// <summary>
    /// Grade composition id.
    /// </summary>
    public int Id { get; set; }
}
