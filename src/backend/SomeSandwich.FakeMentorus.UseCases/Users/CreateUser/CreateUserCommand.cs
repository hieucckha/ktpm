using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.CreateUser;

/// <summary>
/// Command to create a new user.
/// </summary>
public class CreateUserCommand : IRequest
{
    /// <summary>
    /// Email.
    /// </summary>
    required public string Email { get; set; }

    /// <summary>
    /// First name.
    /// </summary>
    required public string FirstName { get; set; }

    /// <summary>
    /// Last name.
    /// </summary>
    required public string LastName { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    required public string Password { get; set; }

    /// <summary>
    /// Student id.
    /// </summary>
    public string? StudentId { get; set; }
}
