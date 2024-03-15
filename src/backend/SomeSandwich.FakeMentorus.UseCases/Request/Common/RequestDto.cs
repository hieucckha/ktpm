using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.UseCases.Comment.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Request.Common;

/// <summary>
///   Request dto.
/// </summary>
public class RequestDto
{
    /// <summary>
    /// The id of the request.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The id of the grade to request.
    /// </summary>
    public int GradeId { get; set; }

    /// <summary>
    /// The name of grade.
    /// </summary>
    public string GradeName { get; set; } = string.Empty;

    /// <summary>
    /// Current grade.
    /// </summary>
    public float CurrentGrade { get; set; }

    /// <summary>
    /// Expected grade.
    /// </summary>
    public float ExpectedGrade { get; set; }

    /// <summary>
    /// Reason of the request.
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// The id of the student who made the request.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// The name of the student who made the request.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// The status of the request.
    /// </summary>
    public RequestStatus Status { get; set; }

    /// <summary>
    /// The date when the request was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date when the request was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Comments.
    /// </summary>
    public virtual ICollection<CommentDto> Comments { get; set; } = null!;

}
