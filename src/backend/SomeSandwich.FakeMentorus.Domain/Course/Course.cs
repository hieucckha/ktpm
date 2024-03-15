using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SomeSandwich.FakeMentorus.Domain.Grade;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Domain.Course;

/// <summary>
/// Course entity.
/// </summary>
public class Course
{
    /// <summary>
    /// Course id.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Course name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the class.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Invite code for the class.
    /// </summary>
    public string? InviteCode { get; set; }

    /// <summary>
    /// Class code for the class.
    /// </summary>
    public string ClassCode { get; set; } = string.Empty;

    /// <summary>
    /// Creator id of the class.
    /// </summary>
    public int? CreatorId { get; set; }

    /// <summary>
    /// Is the class activated.
    /// </summary>
    public bool IsActivated { get; set; } = true;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Course start date.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Course update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Creator of the class.
    /// </summary>
    public User Creator { get; set; } = null!;

    /// <summary>
    /// List students of the class.
    /// </summary>
    public virtual ICollection<CourseStudent> Students { get; set; } = null!;

    /// <summary>
    /// List teachers of the class.
    /// </summary>
    public virtual ICollection<CourseTeacher> Teachers { get; set; } = null!;

    /// <summary>
    /// List of grade compositions of the class.
    /// </summary>
    public virtual ICollection<GradeComposition> GradeCompositions { get; set; } = null!;

    /// <summary>
    /// List of student information of the class.
    /// </summary>
    public virtual ICollection<StudentInfo> StudentInfos { get; set; } = null!;
}
