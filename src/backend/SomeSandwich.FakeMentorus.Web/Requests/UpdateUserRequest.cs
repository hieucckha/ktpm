using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Request to update user.
/// </summary>
public class UpdateUserRequest
{
    // /// <inheritdoc cref="User.Email" />
    // public string? Email { get; set; }

    /// <inheritdoc cref="User.FirstName" />
    public string? FirstName { get; set; }

    /// <inheritdoc cref="User.LastName" />
    public string? LastName { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string? StudentId { get; set; }

    /// <summary>
    /// Avatar file.
    /// </summary>
    public IFormFile? AvatarFile { get; set; }
}
