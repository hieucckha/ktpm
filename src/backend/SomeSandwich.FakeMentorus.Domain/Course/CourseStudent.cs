using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Domain.Course;

/// <summary>
/// Course entity.
/// </summary>
public class CourseStudent
{
    /// <summary>
    /// Course Student id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public int StudentId { get; set; }

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Indicates when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates when the user was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Course.
    /// </summary>
    public virtual Course Course { get; set; } = null!;

    /// <summary>
    /// Student.
    /// </summary>
    public virtual User Student { get; set; } = null!;
}
