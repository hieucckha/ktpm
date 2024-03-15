using MediatR;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.UpdateUserById;

/// <summary>
/// Command to update specific user.
/// </summary>
public class UpdateUserByIdCommand : IRequest
{
    /// <summary>
    /// User id.
    /// </summary>
    public int UserId { get; set; }

    /// <inheritdoc cref="User.FirstName" />
    public string? FirstName { get; set; }

    /// <inheritdoc cref="User.LastName" />
    public string? LastName { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string? StudentId { get; set; }
}
