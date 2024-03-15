using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Request.RejectRequest;

/// <summary>
/// Handler for <see cref="RejectRequestCommand"/>.
/// </summary>
public class RejectRequestCommandHandler : IRequestHandler<RejectRequestCommand>
{
    private readonly IAppDbContext dbContext;
    private readonly INotificationService notificationService;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="notificationService"></param>
    public RejectRequestCommandHandler(IAppDbContext dbContext, INotificationService notificationService)
    {
        this.dbContext = dbContext;
        this.notificationService = notificationService;
    }

    /// <inheritdoc />
    public async Task Handle(RejectRequestCommand command, CancellationToken cancellationToken)
    {
        var request = await dbContext.Requests
            .Include(e => e.Student)
            .FirstOrDefaultAsync(x => x.Id == command.RequestId, cancellationToken);
        if (request == null)
        {
            throw new NotFoundException("Request not found");
        }

        var grade = await dbContext.Grades
            .Include(e => e.GradeComposition)
            .FirstOrDefaultAsync(x => x.Id == request.GradeId, cancellationToken);

        if (grade == null)
        {
            throw new NotFoundException("Grade not found");
        }

        request.Status = RequestStatus.Rejected;
        request.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var course = await dbContext.Courses
            .Where(e => e.GradeCompositions.Any(gc => gc.Id == grade.GradeCompositionId))
            .FirstOrDefaultAsync(cancellationToken);

        await notificationService.SendNotification(request.Student.Email!,
            JsonSerializer.Serialize(new NotificationDto
            {
                Title = "Rejected Request",
                Description = $"Teacher rejected your request in course {course!.Name}",
                ClassId = course!.Id,
                RequestId = request.Id,
                Type = NotificationType.RejectRequest
            }), cancellationToken);
    }
}
