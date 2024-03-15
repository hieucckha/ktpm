namespace SomeSandwich.FakeMentorus.UseCases.Grade.Common;

/// <summary>
/// User in class but not mapping to any student.
/// </summary>
public class UserWithoutStudentDto
{
    /// <summary>
    /// Id of user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Name of user.
    /// </summary>
    public string Name { get; set; }
}
