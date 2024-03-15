using MediatR;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.UpdateCourse;

/// <summary>
/// Handle for <see cref="UpdateCourseCommand"/>.
/// </summary>
internal class UpdateCourseCommandHandle : IRequestHandler<UpdateCourseCommand>
{
    private readonly IAppDbContext dbContext;
    private readonly ILogger<UpdateCourseCommandHandle> logger;

    public UpdateCourseCommandHandle(IAppDbContext dbContext,
        ILogger<UpdateCourseCommandHandle> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating course: {CourseId}", request.CourseId);
        var course =
            await dbContext.Courses.GetAsync(e => e.Id == request.CourseId, cancellationToken);

        course.Name = request.Name ?? course.Name;
        course.Description = request.Description ?? course.Description;

        course.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Course updated: {CourseId}", request.CourseId);
    }
}
