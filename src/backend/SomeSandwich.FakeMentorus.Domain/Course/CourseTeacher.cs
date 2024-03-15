using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Domain.Course;

/// <summary>
/// Course entity.
/// </summary>
public class CourseTeacher
{
    /// <summary>
    /// Course Teacher id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Teacher id.
    /// </summary>
    public int TeacherId { get; set; }

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
    public Course Course { get; set; } = null!;

    /// <summary>
    /// Teacher.
    /// </summary>
    public User Teacher { get; set; } = null!;
}
