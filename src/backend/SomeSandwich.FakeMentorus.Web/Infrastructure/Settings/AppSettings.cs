using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.Settings;

/// <inheritdoc />
public class AppSettings : IAppSettings
{
    /// <inheritdoc />
    public string FrontendUrl { get; set; } = "https://midterm.somesandwich.rocks";
}
