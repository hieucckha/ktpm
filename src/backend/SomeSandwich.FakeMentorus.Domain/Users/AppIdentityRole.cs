using Microsoft.AspNetCore.Identity;

namespace SomeSandwich.FakeMentorus.Domain.Users;

/// <summary>
/// Custom application identity role.
/// </summary>
public class AppIdentityRole : IdentityRole<int>
{
}
