using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ActivateUser.RegisterActivateUser;

/// <summary>
/// Command to generate code to activate user account.
/// </summary>
public class RegisterActivateUserCommand : IRequest
{
    /// <summary>
    /// Email.
    /// </summary>
    [EmailAddress]
    [Required]
    [DataType(DataType.EmailAddress)]
    required public string Email { get; set; }
}
