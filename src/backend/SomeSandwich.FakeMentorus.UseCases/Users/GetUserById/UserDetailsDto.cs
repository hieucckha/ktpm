namespace SomeSandwich.FakeMentorus.UseCases.Users.GetUserById;

/// <summary>
/// User details.
/// </summary>
public class UserDetailsDto
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User first name.
    /// </summary>
    required public string FirstName { get; set; }

    /// <summary>
    /// User last name.
    /// </summary>
    required public string LastName { get; set; }

    /// <summary>
    /// Full user name.
    /// </summary>
    required public string FullName { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    required public string Email { get; set; }

    /// <summary>
    /// Last login date time.
    /// </summary>
    public DateTime LastLogin { get; set; }

    /// <summary>
    /// User role.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Student id.
    /// </summary>
    public string? StudentId { get; set; }
}
