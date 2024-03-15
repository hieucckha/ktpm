using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.Implements;
using SomeSandwich.FakeMentorus.Web.Hubs;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.DependencyInjection;

/// <summary>
/// Application specific dependencies.
/// </summary>
internal static class ApplicationModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="configuration">Configuration.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IFacebookClient, FacebookClient>();
        services.AddTransient<INotificationService, NotificationHub>();
    }
}
