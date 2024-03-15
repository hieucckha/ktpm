namespace SomeSandwich.FakeMentorus.UseCases.Courses.UpdateCourse;

/// <summary>
/// Update course request.
/// </summary>
public class UpdateCourseRequest
{
    /// <summary>
    /// Name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }
}
