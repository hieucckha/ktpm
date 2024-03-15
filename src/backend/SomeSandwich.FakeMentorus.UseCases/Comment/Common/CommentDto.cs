namespace SomeSandwich.FakeMentorus.UseCases.Comment.Common;

/// <summary>
/// Comment dto.
/// </summary>
public class CommentDto
{
    /// <summary>
    /// CommentRequest id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Request id.
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// User id.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Full name of user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Comment details.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Is teacher.
    /// </summary>
    public bool IsTeacher { get; set; } = false;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Created at.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updated at.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
