using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomeSandwich.FakeMentorus.Domain.Grade;

/// <summary>
/// GradeComposition entity.
/// </summary>
public class GradeComposition
{
    /// <summary>
    /// Grade Composition id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Grade Composition name.
    /// </summary>
    required public string? Name { get; set; }

    /// <summary>
    /// Description of the Grade Composition.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Grade Scale of the Grade Composition.
    /// </summary>
    required public int GradeScale { get; set; }

    /// <summary>
    /// Grade Composition order.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Is Grade Composition final?.
    /// </summary>
    public bool IsFinal { get; set; } = false;

    /// <summary>
    /// Grade Composition is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Grade Composition start date.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Grade Composition update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// List of grades of the Grade Composition.
    /// </summary>
    public virtual ICollection<Grade> Grades { get; set; } = null!;

    /// <summary>
    /// Course.
    /// </summary>
    public virtual Course.Course Course { get; set; } = null!;
}
