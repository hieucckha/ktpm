using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.RefreshToken;

/// <summary>
/// Handler for <see cref="RefreshTokenCommand" />.
/// </summary>
internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenModel>
{
    private readonly IAuthenticationTokenService tokenService;
    private readonly SignInManager<User> signInManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    public RefreshTokenCommandHandler(
        IAuthenticationTokenService tokenService,
        SignInManager<User> signInManager)
    {
        this.tokenService = tokenService;
        this.signInManager = signInManager;
    }

    /// <inheritdoc />
    public async Task<TokenModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Get user.
        var userId = GetTokenUserId(request.Token);
        var user = await signInManager.UserManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new DomainException($"User with identifier {userId} not found.");
        }

        // Validate token.
        var tokenCreationDate = GetTokenCreationDate(request.Token);
        if (tokenCreationDate + AuthenticationConstants.RefreshTokenExpire <= DateTime.UtcNow ||
            tokenCreationDate < user.LastTokenResetAt)
        {
            throw new DomainException("Token has been expired.");
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return TokenModelGenerator.Generate(tokenService, principal.Claims);
    }

    private DateTime GetTokenCreationDate(string token)
    {
        var tokenClaims = GetTokenClaims(token);
        var iatClaim = tokenClaims.FirstOrDefault(c => c.Type == AuthenticationConstants.IatClaimType);
        if (iatClaim == null)
        {
            throw new DomainException("Iat claim cannot be found. Invalid token.");
        }

        var epochExpirationDiff = TimeSpan.FromSeconds(long.Parse(iatClaim.Value));
        return DateTime.UnixEpoch + epochExpirationDiff;
    }

    private string GetTokenUserId(string token)
    {
        var tokenClaims = GetTokenClaims(token);
        var userIdClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new DomainException(
                "User identifier claim cannot be found. Invalid token.");
        }
        return userIdClaim.Value;
    }

    private IEnumerable<Claim> GetTokenClaims(string token)
    {
        try
        {
            return tokenService.GetTokenClaims(token);
        }
        catch (Exception)
        {
            throw new DomainException("Invalid token.");
        }
    }
}
