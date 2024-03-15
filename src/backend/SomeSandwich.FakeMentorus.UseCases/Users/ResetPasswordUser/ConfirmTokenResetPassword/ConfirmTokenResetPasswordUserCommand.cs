using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ResetPasswordUser.ConfirmTokenResetPassword;

/// <summary>
/// Confirm token reset password user command.
/// </summary>
public class ConfirmTokenResetPasswordUserCommand : IRequest
{
    /// <summary>
    /// Email.
    /// </summary>
    [EmailAddress]
    [Required]
    [DataType(DataType.EmailAddress)]
    required public string Email { get; set; }

    /// <summary>
    /// Code to reset password.
    /// </summary>
    required public string Code { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    required public string Password { get; set; }

    /// <summary>
    /// Confirm password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    required public string ConfirmPassword { get; set; }
}
