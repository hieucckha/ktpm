using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Comment.Common;
using SomeSandwich.FakeMentorus.UseCases.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Comment.CreateComment;

/// <summary>
/// Handler for <see cref="CreateCommentCommand"/>.
/// </summary>
public class CreateCommentCommandHandle : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly IAppDbContext appDbContext;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="appDbContext"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="mapper"></param>
    /// <param name="notificationService"></param>
    public CreateCommentCommandHandle(IAppDbContext appDbContext, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager, IMapper mapper, INotificationService notificationService)
    {
        this.appDbContext = appDbContext;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
        this.mapper = mapper;
        this.notificationService = notificationService;
    }

    /// <inheritdoc />
    public async Task<CommentDto> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(loggedUserAccessor.GetCurrentUserId().ToString());

        if (user == null)
        {
            throw new DomainException("User not found");
        }

        var request = await appDbContext.Requests
            .AsNoTracking()
            .Include(e => e.Grade)
            .ThenInclude(e => e.GradeComposition)
            .Where(e => e.Id == command.RequestId)
            .FirstOrDefaultAsync(cancellationToken);

        if (request is null)
        {
            throw new DomainException($"Not found request with id {command.RequestId}");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        var comment = new CommentRequest()
        {
            RequestId = command.RequestId, UserId = user.Id, Comment = command.Comment, IsTeacher = role is "Teacher",
        };
        await appDbContext.CommentRequests.AddAsync(comment, cancellationToken);

        var course = await appDbContext.Courses
            .AsNoTracking()
            .Include(e => e.Teachers)
            .ThenInclude(e => e.Teacher)
            .Where(e => e.GradeCompositions.Any(gc => gc.Id == request.Grade.GradeCompositionId))
            .FirstOrDefaultAsync(cancellationToken);

        await appDbContext.SaveChangesAsync(cancellationToken);
        if (role is "Student")
        {
            foreach (var teacher in course!.Teachers)
            {
                await notificationService.SendNotification(teacher.Teacher.Email!,
                    JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = "New Comment",
                        Description = $"Student has comment on request on grade request in course {course.Name}",
                        Type = NotificationType.CreateComment,
                        ClassId = course.Id,
                        RequestId = request.Id
                    }),
                    cancellationToken);
            }
        }
        else
        {
            var students = await appDbContext.CommentRequests
                .AsNoTracking()
                .Include(e => e.User)
                .Where(e => e.RequestId == command.RequestId)
                .Where(e => e.IsTeacher == false)
                .Select(e => e.User)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var student in students)
            {
                await notificationService.SendNotification(student.Email!,
                    JsonSerializer.Serialize(new NotificationDto
                    {
                        Title = "New Comment",
                        Description = $"Teacher has comment on your request in course {course!.Name}",
                        Type = NotificationType.CreateComment,
                        ClassId = course.Id,
                        RequestId = request.Id
                    }),
                    cancellationToken);
            }
        }

        var result = mapper.Map<CommentDto>(comment);
        result.Name = user.FullName;
        return result;
    }
}
