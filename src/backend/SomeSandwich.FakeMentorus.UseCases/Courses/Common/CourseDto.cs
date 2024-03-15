namespace SomeSandwich.FakeMentorus.UseCases.Courses.Common;

/// <summary>
/// Course DTO.
/// </summary>
public class CourseDto
{
    /// <summary>
    /// Course ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Course name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Course description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Is the class activated.
    /// </summary>
    public bool IsActivated { get; set; }

    /// <summary>
    /// Course start date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Course end date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Create by user id.
    /// </summary>
    public int? CreatorId { get; set; }

    /// <summary>
    /// Create by user name.
    /// </summary>
    public string? CreatorName { get; set; } = string.Empty;

    /// <summary>
    /// Number of students in the course.
    /// </summary>
    public int NumberOfStudents { get; set; }

    /// <summary>
    ///   Number of teachers in the course.
    /// </summary>
    public int NumberOfTeachers { get; set; }
}
