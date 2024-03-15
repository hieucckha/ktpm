using Microsoft.AspNetCore.Identity;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.DataAccess;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.Web;

/// <summary>
/// Database initializer.
/// </summary>
public class DbInitializer : IDbInitializer

{
    private readonly AppDbContext appDbContext;
    private readonly RoleManager<AppIdentityRole> roleManager;

    /// <summary>
    /// Database initializer.
    /// </summary>
    /// <param name="appDbContext"></param>
    /// <param name="roleManager"></param>
    public DbInitializer(AppDbContext appDbContext, RoleManager<AppIdentityRole> roleManager)
    {
        this.appDbContext = appDbContext;
        this.roleManager = roleManager;
    }

    /// <summary>
    /// Seed the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var roles = new[] { "Admin", "Student", "Teacher" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppIdentityRole { Name = role });
            }
        }
    }
}
