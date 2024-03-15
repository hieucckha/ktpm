namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Global application settings.
/// </summary>
public interface IAppSettings
{
    /// <summary>
    /// Default value.
    /// </summary>
    public string FrontendUrl { get; set; }
}
