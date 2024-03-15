using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Common;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition.ToggleIsFinished;

/// <summary>
/// Handle toggle is finished command.
/// </summary>
public class ToggleIsFinishedCommandHandle : IRequestHandler<ToggleIsFinishedCommand>
{
    private readonly IAppDbContext dbContext;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;
    private readonly INotificationService notificationService;


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="notificationService"></param>
    public ToggleIsFinishedCommandHandle(IAppDbContext dbContext, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager, INotificationService notificationService)
    {
        this.dbContext = dbContext;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
        this.notificationService = notificationService;
    }

    /// <inheritdoc />
    public async Task Handle(ToggleIsFinishedCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(loggedUserAccessor.GetCurrentUserId().ToString());
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is not "Teacher")
        {
            throw new ForbiddenException("Only teacher can toggle is finished.");
        }

        var check = await dbContext.CourseTeachers
            .Include(e => e.Course)
            .ThenInclude(c => c.GradeCompositions)
            .Where(e => e.Course.GradeCompositions.Any(x => x.Id == request.Id))
            .AnyAsync(e => e.TeacherId == user.Id, cancellationToken);
        if (!check)
        {
            throw new ForbiddenException("Only teacher who teach this course can toggle is finished.");
        }

        var gradeComposition =
            await dbContext.GradeCompositions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (gradeComposition == null)
        {
            throw new NotFoundException($"Grade composition with id {request.Id} not found.");
        }

        gradeComposition.IsFinal = !gradeComposition.IsFinal;
        gradeComposition.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        if (gradeComposition.IsFinal == true)
        {
            var course = await dbContext.Courses
                .Include(e => e.Students)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(e => e.Id == gradeComposition.CourseId, cancellationToken);

            if (course == null)
            {
                throw new NotFoundException($"Course with id {gradeComposition.CourseId} not found.");
            }

            foreach (var st in course.Students)
            {
                await notificationService.SendNotification(st.Student.Email!,
                    JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = "Grade composition is finalized.",
                        Description =
                            $"Grade composition {gradeComposition.Name} of course {gradeComposition.Course.Name} is final, you can see your grade.",
                        ClassId = course.Id,
                        Type = NotificationType.FinalizedGradeComposition
                    }),
                    cancellationToken);
            }
        }
    }
}
