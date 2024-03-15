using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Common.Service;

/// <summary>
/// Access service.
/// </summary>
public class AccessService : IAccessService
{
    private readonly IAppDbContext dbContext;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    public AccessService(IAppDbContext dbContext, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> HasAccessToCourse(int courseId, CancellationToken cancellationToken)
    {
        var loggedUserId = loggedUserAccessor.GetCurrentUserId();
        var user = await dbContext.Users.GetAsync(u => u.Id == loggedUserId, cancellationToken);
        var userRole = (await userManager.GetRolesAsync(user))
            .FirstOrDefault();

        return userRole switch
        {
            "Admin" => true,
            "Student" => await dbContext.CourseStudents.AnyAsync(
                c => c.StudentId == loggedUserId && c.CourseId == courseId, cancellationToken),
            "Teacher" => await dbContext.CourseTeachers.AnyAsync(
                c => c.TeacherId == loggedUserId && c.CourseId == courseId, cancellationToken),
            _ => false
        };
    }
}
