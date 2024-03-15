using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Common;
using SomeSandwich.FakeMentorus.UseCases.Request.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Request.CreateRequest;

/// <summary>
/// Handle for <see cref="CreateRequestCommand"/>
/// </summary>
public class CreateRequestCommandHandle : IRequestHandler<CreateRequestCommand, RequestDto>
{
    private readonly IAppDbContext dbContext;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Constructor for <see cref="CreateRequestCommandHandle"/>.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="mapper"></param>
    /// <param name="notificationService"></param>
    public CreateRequestCommandHandle(IAppDbContext dbContext, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager, IMapper mapper, INotificationService notificationService)
    {
        this.dbContext = dbContext;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
        this.mapper = mapper;
        this.notificationService = notificationService;
    }

    /// <inheritdoc />
    public async Task<RequestDto> Handle(CreateRequestCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(loggedUserAccessor.GetCurrentUserId().ToString());
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is not "Student")
        {
            throw new ForbiddenException("Only students can create requests");
        }

        if (user.StudentId == null)
        {
            throw new NotFoundException("Student info not found");
        }

        var grade = await dbContext.Grades
            .Where(e => e.GradeCompositionId == command.GradeCompositionId)
            .Where(e => e.StudentId == user.StudentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (grade == null)
        {
            throw new NotFoundException("Grade not found");
        }

        if (grade.IsRequested == true)
        {
            throw new DomainException("You already have request for this grade");
        }

        var requestEntity = new Domain.Request.Request
        {
            StudentId = user.Id,
            GradeId = grade.Id,
            Reason = command.Reason,
            ExpectedGrade = command.ExceptedGrade,
            CurrentGrade = grade.GradeValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await dbContext.Requests.AddAsync(requestEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        grade.IsRequested = true;
        grade.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var result = mapper.Map<RequestDto>(requestEntity);

        var course = await dbContext.Courses
            .Include(c => c.StudentInfos)
            .ThenInclude(e => e.Student)
            .ThenInclude(s => s.User)
            .Include(e => e.Teachers)
            .ThenInclude(e => e.Teacher)
            .Include(c => c.GradeCompositions)
            .ThenInclude(gc => gc.Grades)
            .Where(c => c.GradeCompositions.Any(gc =>
                gc.Grades.Any(g => g.Id == grade.Id) && gc.IsDeleted != true))
            .Where(c => c.StudentInfos.Any(si => si.Student.User.Id == user.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (course == null)
        {
            result.StudentName = "";
        }
        else
        {
            result.StudentName = course.StudentInfos
                .Where(si => si.StudentId == user.StudentId)
                .Select(si => si.Name)
                .FirstOrDefault() ?? "";

            foreach (var courseTeacher in course.Teachers)
            {
                await notificationService.SendNotification(courseTeacher.Teacher.Email!,
                    JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = $"New grade request",
                        Description = $"Student {result.StudentName} create new request in course {course.Name}",
                        ClassId = course.Id,
                        RequestId = requestEntity.Id,
                        Type = NotificationType.CreateRequest
                    }), cancellationToken);
            }
        }

        return result;
    }
}
