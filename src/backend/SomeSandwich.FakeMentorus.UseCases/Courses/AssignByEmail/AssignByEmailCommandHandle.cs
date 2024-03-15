using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.AssignByEmail;

/// <summary>
/// Handler for <see cref="AssignByEmailCommand" />.
/// </summary>
internal class AssignByEmailCommandHandle : IRequestHandler<AssignByEmailCommand>
{
    private readonly IMemoryCache memoryCache;
    private readonly ILogger<AssignByEmailCommandHandle> logger;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;
    private readonly IAppDbContext dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="memoryCache"></param>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    /// <param name="dbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    public AssignByEmailCommandHandle(
        IMemoryCache memoryCache,
        ILogger<AssignByEmailCommandHandle> logger,
        ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager,
        IAppDbContext dbContext)
    {
        this.memoryCache = memoryCache;
        this.logger = logger;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task Handle(AssignByEmailCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = loggedUserAccessor.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(currentUserId.ToString());

        var cacheInviteValue = ValidateToken(command.Token);
        memoryCache.Remove(command.Token);

        if (cacheInviteValue.Email != user?.Email)
        {
            logger.LogInformation(
                "The logged-in user's email {LoggedEmail} is not the same as the token email {TokenEmail} ",
                user!.Email,
                cacheInviteValue.Email);
            throw new NotFoundException("Invitation link is for wrong person, Please try different account.");
        }

        var course = await dbContext.Courses
            .Include(c => c.Students)
            .Include(c => c.Teachers)
            .GetAsync(c => c.Id == cacheInviteValue.CourseId, cancellationToken);

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        switch (role)
        {
            case "Student" when course.Students.Any(s => s.StudentId == currentUserId):
                logger.LogWarning("Student with id {CurrentUserId} already assigned to course with id {CourseId}",
                    currentUserId, cacheInviteValue.CourseId);
                throw new DomainException("You are already assigned to this course.");
            case "Student":
                dbContext.CourseStudents.Add(new CourseStudent { CourseId = course.Id, StudentId = currentUserId });
                if (user.StudentId is not null)
                {
                    await dbContext.StudentInfos.AddAsync(new StudentInfo
                    {
                        CourseId = course.Id,
                        Name = user.FullName,
                        StudentId = user.StudentId
                    }, cancellationToken);
                }
                break;
            case "Teacher" when course.Teachers.Any(t => t.TeacherId == currentUserId):
                logger.LogWarning(
                    "Teacher with id {TeacherId} already assigned to course with id {CourseId}",
                    currentUserId, cacheInviteValue.CourseId);
                throw new DomainException("You are already assigned to this course.");
            case "Teacher":
                dbContext.CourseTeachers.Add(new CourseTeacher { CourseId = course.Id, TeacherId = currentUserId });
                break;
            default:
                logger.LogWarning("User with id {UserId} is admin", currentUserId);
                throw new DomainException("You are not allowed to assign to this course.");
                break;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User with id {UserId} assigned to course with id {CourseId}",
            currentUserId, cacheInviteValue.CourseId);
    }

    private CacheInviteValue ValidateToken(string token)
    {
        memoryCache.TryGetValue(token, out var cacheValue);
        if (cacheValue is null)
        {
            logger.LogInformation("Token {Token} not found", token);
            throw new NotFoundException("Invitation link is expired");
        }

        var cacheInviteValue = cacheValue as CacheInviteValue;
        if (cacheInviteValue is null)
        {
            logger.LogInformation("Type of token {Token} is not valid: {Type}", token,
                typeof(CacheInviteValue));
            throw new Exception("Invitation link is not valid");
        }

        return cacheInviteValue;
    }
}
