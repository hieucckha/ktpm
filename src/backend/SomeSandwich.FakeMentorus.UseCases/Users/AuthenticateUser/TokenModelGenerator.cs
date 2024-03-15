using System.Security.Claims;

namespace SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser;

/// <summary>
/// Helper to generate <see cref="TokenModel" />.
/// </summary>
internal static class TokenModelGenerator
{
    /// <summary>
    /// Common code to generate token and fill with claims.
    /// </summary>
    /// <param name="authenticationTokenService">Authentication token service.</param>
    /// <param name="claims">User claims.</param>
    /// <returns>Token model.</returns>
    public static TokenModel Generate(
        IAuthenticationTokenService authenticationTokenService,
        IEnumerable<Claim> claims)
    {
        var epoch = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
        var iatClaim = new Claim(
            AuthenticationConstants.IatClaimType,
            epoch.ToString(), ClaimValueTypes.Integer64);

        return new TokenModel
        {
            Token = authenticationTokenService.GenerateToken(
                claims.Union(new[] { iatClaim }),
                AuthenticationConstants.AccessTokenExpirationTime),
            ExpiresIn = (int)AuthenticationConstants.AccessTokenExpirationTime.TotalSeconds
        };
    }
}
