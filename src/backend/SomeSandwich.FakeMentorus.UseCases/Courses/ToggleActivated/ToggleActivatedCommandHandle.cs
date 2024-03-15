using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.ToggleActivated;

/// <summary>
/// Handles the <see cref="ToggleActivatedCommand"/> by toggling the activated state of the course.
/// </summary>
public class ToggleActivatedCommandHandle : IRequestHandler<ToggleActivatedCommand>
{
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor for <see cref="ToggleActivatedCommandHandle"/>.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    public ToggleActivatedCommandHandle(IAppDbContext dbContext, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task Handle(ToggleActivatedCommand command, CancellationToken cancellationToken)
    {
        var userId = loggedUserAccessor.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is not "Teacher" && role is not "Admin")
        {
            throw new ForbiddenException("Only teacher or admin can toggle course activation");
        }

        var course = await dbContext.Courses
            .Include(c => c.Teachers)
            .Where(c => c.Id == command.CourseId)
            .FirstOrDefaultAsync(cancellationToken);

        if (course == null)
        {
            throw new NotFoundException("Course not found");
        }

        if (course.Teachers.Any(t => t.TeacherId == userId))
        {
            throw new ForbiddenException("Only teachers of the course can toggle course activation");
        }

        course.IsActivated = !course.IsActivated;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
