using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Dtos;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.Infrastructure.Implements;

/// <inheritdoc cref="SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces.IFacebookClient" />
public class FacebookClient : IFacebookClient, IDisposable
{
    private readonly ILogger<FacebookClient> logger;
    private readonly RestClient client;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    public FacebookClient(ILogger<FacebookClient> logger)
    {
        this.logger = logger;

        var option = new RestClientOptions("https://graph.facebook.com");
        client = new RestClient(option, configureSerialization: s => s.UseNewtonsoftJson());
    }

    /// <inheritdoc />
    public async Task<FaceBookUserInformationDto> GetCurrentUserInformationAsync(string accessToken,
        CancellationToken cancellationToken)
    {
        var request = new RestRequest("me")
            .AddQueryParameter("access_token", accessToken)
            .AddQueryParameter("fields", "email");

        var response = await client.ExecuteGetAsync<FaceBookUserInformationDto>(request, cancellationToken);

        response.ThrowIfError();

        return response.Data!;
    }

    private bool disposed;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True if call from Dispose method, false if call from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                client.Dispose();
            }

            disposed = true;
        }
    }
}
