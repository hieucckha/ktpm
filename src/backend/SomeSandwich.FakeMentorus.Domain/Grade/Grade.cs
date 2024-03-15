using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomeSandwich.FakeMentorus.Domain.Grade;

/// <summary>
/// Grade entity.
/// </summary>
public class Grade
{
    /// <summary>
    /// Grade id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Grade Composition id.
    /// </summary>
    required public int GradeCompositionId { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string StudentId { get; set; }

    /// <summary>
    /// Grade value.
    /// </summary>
    required public float GradeValue { get; set; }

    public bool IsRequested { get; set; } = false;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Grade comment.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Updated at.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Grade Composition.
    /// </summary>
    public virtual GradeComposition GradeComposition { get; set; } = null!;

    /// <summary>
    /// Student.
    /// </summary>
    public virtual Student.Student Student { get; set; } = null!;

    /// <summary>
    /// Request.
    /// </summary>
    public virtual Request.Request Request { get; set; } = null!;
}
