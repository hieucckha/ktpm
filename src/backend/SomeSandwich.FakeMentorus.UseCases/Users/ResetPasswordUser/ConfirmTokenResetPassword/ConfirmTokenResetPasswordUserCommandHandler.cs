using MediatR;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ResetPasswordUser.ConfirmTokenResetPassword;

/// <summary>
/// Handler for <see cref="ConfirmTokenResetPasswordUserCommand"/>.
/// </summary>
public class ConfirmTokenResetPasswordUserCommandHandler : IRequestHandler<ConfirmTokenResetPasswordUserCommand>
{
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    public ConfirmTokenResetPasswordUserCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(ConfirmTokenResetPasswordUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var result = await userManager.ResetPasswordAsync(user, command.Code, command.Password);
        if (!result.Succeeded)
        {
            throw new DomainException("Invalid code");
        }
    }
}
