using SomeSandwich.FakeMentorus.UseCases.Users.AuthenticateUser.LoginUser;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.DependencyInjection;

/// <summary>
/// Register Mediator as dependency.
/// </summary>
internal static class MediatRModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly));
    }
}
