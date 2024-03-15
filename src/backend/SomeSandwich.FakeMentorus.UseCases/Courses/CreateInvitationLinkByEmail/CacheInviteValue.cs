namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;

/// <summary>
///  Cache value for invitation link.
/// </summary>
public class CacheInviteValue
{
    /// <summary>
    /// Course id to create invitation link.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    ///  Email of user to invite.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="email"></param>
    public CacheInviteValue(int courseId, string email)
    {
        CourseId = courseId;
        Email = email;
    }
}
