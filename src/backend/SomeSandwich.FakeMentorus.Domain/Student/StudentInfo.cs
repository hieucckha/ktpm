using System.ComponentModel.DataAnnotations;

namespace SomeSandwich.FakeMentorus.Domain.Student;

/// <summary>
/// Student Information Entity.
/// </summary>
public class StudentInfo
{
    /// <summary>
    /// Student Information Id.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Student id real.
    /// </summary>
    required public string StudentId { get; set; }

    /// <summary>
    /// Student.
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Course id.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary
    /// Course.
    /// </summary>
    public Course.Course Course { get; set; } = null!;

    /// <summary>
    /// Name of student.
    /// </summary>
    required public string Name { get; set; }
}
