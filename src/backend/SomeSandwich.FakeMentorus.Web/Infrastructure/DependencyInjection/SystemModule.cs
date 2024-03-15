using Microsoft.AspNetCore.Mvc.Rendering;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.DataAccess;
using SomeSandwich.FakeMentorus.UseCases.Common.Service;
using SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Jwt;
using SomeSandwich.FakeMentorus.Web.Infrastructure.Web;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.DependencyInjection;

/// <summary>
/// System specific dependencies.
/// </summary>
internal static class SystemModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IJsonHelper, SystemTextJsonHelper>();
        services.AddScoped<IAuthenticationTokenService, SystemJwtTokenService>();
        services.AddScoped<IAppDbContext>(s => s.GetRequiredService<AppDbContext>());
        services.AddScoped<ILoggedUserAccessor, LoggedUserAccessor>();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddScoped<IAccessService, AccessService>();
    }
}
