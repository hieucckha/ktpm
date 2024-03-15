using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ResetPasswordUser.GenerateTokenResetPasswordUser;

/// <summary>
/// Generate a token for resetting the password of a user command.
/// </summary>
public class GenerateTokenResetPasswordUserCommand : IRequest
{
    /// <summary>
    /// Email.
    /// </summary>
    [EmailAddress]
    [Required]
    [DataType(DataType.EmailAddress)]
    required public string Email { get; set; }
}
