using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Comment.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Comment.CreateComment;

/// <summary>
/// Create comment command.
/// </summary>
public class CreateCommentCommand : IRequest<CommentDto>
{
    /// <summary>
    /// Request id.
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// Comment details.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
