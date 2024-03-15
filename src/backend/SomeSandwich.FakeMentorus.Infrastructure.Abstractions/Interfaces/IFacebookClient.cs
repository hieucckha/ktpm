using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Dtos;

namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Facebook client.
/// </summary>
public interface IFacebookClient
{
    /// <summary>
    /// Retrieves the current user's information from Facebook using the provided access token.
    /// </summary>
    /// <param name="accessToken">Access token.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns></returns>
    Task<FaceBookUserInformationDto> GetCurrentUserInformationAsync(string accessToken, CancellationToken cancellationToken);
}
