using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ActivateUser.ConfirmActivateUser;

/// <summary>
/// Confirm activate user command.
/// </summary>
public class ConfirmActivateUserCommand : IRequest
{
    /// <summary>
    /// Email.
    /// </summary>
    [EmailAddress]
    [Required]
    [DataType(DataType.EmailAddress)]
    required public string Email { get; set; }

    /// <summary>
    /// Code to activate user account.
    /// </summary>
    [Required]
    required public string Code { get; set; }
}
