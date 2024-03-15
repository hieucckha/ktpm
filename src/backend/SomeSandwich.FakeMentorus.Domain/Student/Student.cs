using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Domain.Student;

/// <summary>
/// Student entity.
/// </summary>
public class Student
{
    /// <summary>
    /// Student's id.
    /// </summary>
    [Key]
    required public string StudentId { get; set; }

    /// <summary>
    /// User.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Student info.
    /// </summary>
    public ICollection<StudentInfo> StudentInfo { get; set; } = null!;

    /// <summary>
    /// Grades.
    /// </summary>
    public ICollection<Grade.Grade> Grades { get; set; } = null!;
}
