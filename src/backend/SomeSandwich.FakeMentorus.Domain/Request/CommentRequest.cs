using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Domain.Request;

/// <summary>
/// CommentRequest entity.
/// </summary>
public class CommentRequest
{
    /// <summary>
    /// CommentRequest id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Grade.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Request.
    /// </summary>
    public virtual Request Request { get; set; } = null!;
}
