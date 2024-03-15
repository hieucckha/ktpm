using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.UseCases.Comment.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Request.GetRequestById;

public class RequestDetailDto
{
    /// <summary>
    /// Request id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Grade id.
    /// </summary>
    public int GradeId { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// The name of the student who made the request.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Current grade.
    /// </summary>
    public float CurrentGrade { get; set; }

    /// <summary>
    /// Expected grade.
    /// </summary>
    public float ExpectedGrade { get; set; }

    /// <summary>
    /// Request detail.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Request status.
    /// </summary>
    public RequestStatus Status { get; set; }

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Created at.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updated at.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Comments.
    /// </summary>
    public virtual ICollection<CommentDto> Comments { get; set; } = null!;
}
