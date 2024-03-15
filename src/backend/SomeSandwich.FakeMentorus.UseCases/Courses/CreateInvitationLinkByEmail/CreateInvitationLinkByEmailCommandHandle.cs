using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.Record;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using shortid.Configuration;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;

/// <summary>
/// Handler for <see cref="CreateInvitationLinkByEmailCommand" />.
/// </summary>
internal class CreateInvitationLinkByEmailCommandHandle :
    IRequestHandler<CreateInvitationLinkByEmailCommand>
{
    private readonly IMemoryCache cache;
    private readonly IAppSettings appSettings;
    private readonly ILogger<CreateInvitationLinkByEmailCommandHandle> logger;
    private readonly IEmailSender emailSender;
    private readonly IAppDbContext dbContext;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cache">Cache instance.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="emailSender">Email sender service instance.</param>
    /// <param name="dbContext">Database context instance.</param>
    /// <param name="userManager"></param>
    /// <param name="appSettings"></param>
    public CreateInvitationLinkByEmailCommandHandle(
        IMemoryCache cache,
        ILogger<CreateInvitationLinkByEmailCommandHandle> logger,
        IEmailSender emailSender,
        IAppDbContext dbContext,
        UserManager<User> userManager,
        IAppSettings appSettings)
    {
        this.cache = cache;
        this.logger = logger;
        this.emailSender = emailSender;
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.appSettings = appSettings;
    }

    /// <inheritdoc />
    public async Task Handle(CreateInvitationLinkByEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .GetAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("Email not found");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        var course = await dbContext.Courses
            .Include(e => e.Students)
            .Include(e => e.Teachers)
            .GetAsync(x => x.Id == request.CourseId, cancellationToken);
        if (course == null)
        {
            logger.LogWarning("Course {CourseID} not found", request.CourseId);
            throw new NotFoundException("Course not found");
        }

        if (role is not null)
        {
            switch (role)
            {
                case "Student":
                    if (course.Students.Any(s => s.StudentId == user.Id))
                    {
                        throw new DomainException("User is already student in this course");
                    }
                    break;
                case "Teacher":
                    if (course.Teachers.Any(s => s.TeacherId == user.Id))
                    {
                        throw new DomainException("User is already teacher in this course");
                    }
                    break;
                default:
                    throw new DomainException("User is not student or teacher");
            }
        }

        var cacheKey = shortid.ShortId
            .Generate(new GenerationOptions(true, false, 10));
        var cacheValue = new CacheInviteValue(request.CourseId, request.Email);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(15));

        cache.Set(cacheKey, cacheValue, cacheEntryOptions);
        logger.LogInformation("Invitation link created with {Token}", cacheKey);

        // Todo: Need url from frontend

        var urlOfSendMail = $"{appSettings.FrontendUrl}/course/invite-email/confirm/{cacheKey}";

        await emailSender.SendEmailAsync(
            $"<div>Please <a href='{urlOfSendMail}'>clicking here</a> to this course.</div>",
            $"You has a invite to {course.Name}",
            new List<string> { request.Email! }, cancellationToken);
    }
}
