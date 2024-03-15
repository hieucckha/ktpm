using MediatR;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.ActivateUser.ConfirmActivateUser;

/// <summary>
/// Handler for <see cref="ConfirmActivateUserCommand"/>.
/// </summary>
public class ConfirmActivateUserCommandHandler : IRequestHandler<ConfirmActivateUserCommand>
{
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    public ConfirmActivateUserCommandHandler(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(ConfirmActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Code);
        if (!result.Succeeded)
        {
            throw new DomainException("Invalid code");
        }
    }
}
