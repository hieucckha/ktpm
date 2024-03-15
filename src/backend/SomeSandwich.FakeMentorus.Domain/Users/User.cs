using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.Common.Utils;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.Domain.Request;

namespace SomeSandwich.FakeMentorus.Domain.Users;

/// <summary>
/// Custom application user entity.
/// </summary>
public class User : IdentityUser<int>
{
    /// <summary>
    /// First name.
    /// </summary>
    [MaxLength(255)]
    [Required]
    required public string FirstName { get; set; }

    /// <summary>
    /// Last name.
    /// </summary>
    [MaxLength(255)]
    [Required]
    required public string LastName { get; set; }

    /// <summary>
    /// Full name, concat of first name and last name.
    /// </summary>
    public string FullName => StringUtils.JoinIgnoreEmpty(" ", FirstName, LastName);

    /// <summary>
    /// The date when user last logged in.
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Last token reset date. Before the date all generate login tokens are considered
    /// not valid. Must be in UTC format.
    /// </summary>
    public DateTime LastTokenResetAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Avatar Path.
    /// </summary>
    public string? AvatarPath { get; set; }

    // /// <summary>
    // /// School Identity.
    // /// </summary>
    // public string? SchoolId { get; set; }

    /// <summary>
    /// Student Identity.
    /// </summary>
    public string? StudentId { get; set; }

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Indicates when the user was updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Indicates when the user was removed.
    /// </summary>
    public DateTime? RemovedAt { get; set; }

    // ---------------------------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    public virtual ICollection<CourseStudent> ClassesStudent { get; set; } = null!;

    /// <summary>
    /// </summary>
    public virtual ICollection<CourseTeacher> ClassesTeacher { get; set; } = null!;

    /// <summary>
    /// List of requests.
    /// </summary>
    public virtual ICollection<Request.Request> Requests { get; set; } = null!;

    /// <summary>
    /// List of comment requests.
    /// </summary>
    public virtual ICollection<CommentRequest> CommentRequests { get; set; } = null!;

    /// <summary>
    /// List of courses.
    /// </summary>
    public virtual ICollection<Course.Course> Courses { get; set; } = null!;

    /// <summary>
    /// Student.
    /// </summary>
    public Student.Student Student { get; set; } = null!;
}
