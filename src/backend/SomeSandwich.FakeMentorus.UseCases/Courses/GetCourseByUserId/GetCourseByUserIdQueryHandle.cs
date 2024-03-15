using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;
using SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseById;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseByUserId;

/// <summary>
/// Handle for <see cref="GetCourseByUserIdQuery"/>.
/// </summary>
public class GetCourseByUserIdQueryHandle : IRequestHandler<GetCourseByUserIdQuery,
    IEnumerable<CourseDto>>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;
    private readonly ILogger<GetCourseByIdQueryHandle> logger;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"> </param>
    public GetCourseByUserIdQueryHandle(IAppDbContext dbContext, IMapper mapper,
        ILogger<GetCourseByIdQueryHandle> logger, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.logger = logger;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CourseDto>> Handle(GetCourseByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Get list courses by user id: {UserId}", request.UserId);
        var userId = loggedUserAccessor.GetCurrentUserId();
        var user = await dbContext.Users.GetAsync(u => u.Id == request.UserId, cancellationToken);
        var roles = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var courses = new List<Domain.Course.Course>();

        switch (roles)
        {
            case "Student":
                courses = await dbContext.Courses.Include(e => e.Students)
                    .Include(e => e.Teachers)
                    .Where(e => e.Students.Any(c => e.Id == userId)).ToListAsync(cancellationToken);
                break;
            case "Teacher":
                courses = await dbContext.Courses.Include(e => e.Teachers)
                    .Include(e => e.Students)
                    .Where(e => e.Teachers.Any(c => e.Id == userId)).ToListAsync(cancellationToken);
                break;
        }


        var result = mapper.Map<List<Domain.Course.Course>, List<CourseDto>>(courses);

        return result;
    }
}
