using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.DataAccess;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.Startup;

/// <summary>
/// Contains database migration helper methods.
/// </summary>
internal sealed class DatabaseInitializer : IAsyncInitializer
{
    private readonly AppDbContext appDbContext;
    private readonly IDbInitializer dbInitializer;

    /// <summary>
    /// Database initializer. Performs migration and data seed.
    /// </summary>
    /// <param name="appDbContext">Data context.</param>
    /// <param name="dbInitializer"></param>
    public DatabaseInitializer(AppDbContext appDbContext, IDbInitializer dbInitializer)
    {
        this.appDbContext = appDbContext;
        this.dbInitializer = dbInitializer;
    }

    /// <inheritdoc />
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        await appDbContext.Database.MigrateAsync(cancellationToken);

        // Seed Role data.
        await dbInitializer.SeedAsync(cancellationToken);
    }
}
