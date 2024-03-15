using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Common.Utils;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore.Pagination;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.SearchCourse;

/// <summary>
/// Handle for <see cref="SearchCoursesQuery"/>.
/// </summary>
internal class SearchCoursesQueryHandle : IRequestHandler<SearchCoursesQuery, PagedList<CourseDto>>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    /// <param name="userManager"></param>
    /// <param name="loggedUserAccessor"></param>
    public SearchCoursesQueryHandle(IAppDbContext dbContext, IMapper mapper, UserManager<User> userManager,
        ILoggedUserAccessor loggedUserAccessor)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.userManager = userManager;
        this.loggedUserAccessor = loggedUserAccessor;
    }

    public async Task<PagedList<CourseDto>> Handle(SearchCoursesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Courses
            .Include(c => c.Creator)
            .Include(c => c.Students)
            .Include(c => c.Teachers)
            .AsQueryable();
        if (request.UserId != null)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString()!);
            var role = (await userManager.GetRolesAsync(user ?? throw new NotFoundException("User not found")))
                .FirstOrDefault();
            query = role switch
            {
                "Student" => query.Where(q => q.Students.Any(s => s.StudentId == request.UserId)),
                "Teacher" => query.Where(q => q.Teachers.Any(t => t.TeacherId == request.UserId)),
                _ => query.Where(q=>q.Students.Any(s=>s.StudentId == request.UserId))
            };
        }

        query = CollectionUtils.OrderMultiple(
            query,
            OrderParsingDelegates.ParseSeparated(request.OrderBy.ToLower()),
            ("id", x => x.Id),
            ("creatorId", x => x.CreatorId!),
            ("name", x => x.Name)
        );

        var pagedList =
            await EFPagedListFactory.FromSourceAsync(query, request.Page, request.PageSize, cancellationToken);

        var result = pagedList.Convert(c => mapper.Map<CourseDto>(c));
        var queryResult = await query.ToListAsync(cancellationToken);
        foreach (var courseDto in result)
        {
            courseDto.CreatorName = queryResult.FirstOrDefault(c => c.Id == courseDto.Id)?.Creator?.FullName ?? "";
        }
        return result;
    }
}
