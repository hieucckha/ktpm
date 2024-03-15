namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
///     Database initializer.
/// </summary>
public interface IDbInitializer
{
    /// <summary>
    ///     Seed the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SeedAsync(CancellationToken cancellationToken = default);
}
