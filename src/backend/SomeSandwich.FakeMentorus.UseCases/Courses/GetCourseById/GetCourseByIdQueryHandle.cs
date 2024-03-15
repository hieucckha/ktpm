using System.Security.Cryptography;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.Util;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Common.Service;
using SomeSandwich.FakeMentorus.UseCases.Request.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseById;

/// <summary>
/// Handler for <see cref="GetCourseByIdQuery" />.
/// </summary>
public class GetCourseByIdQueryHandle : IRequestHandler<GetCourseByIdQuery, CourseDetailDto>
{
    private readonly ILogger<GetCourseByIdQueryHandle> logger;
    private readonly IAppDbContext dbContext;
    private readonly IAccessService accessService;
    private readonly IAppSettings appSettings;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="dbContext">Database context instance.</param>
    /// <param name="accessService">Access service instance.</param>
    /// <param name="appSettings">App settings instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    /// <param name="userManager"></param>
    public GetCourseByIdQueryHandle(
        IAppDbContext dbContext,
        IMapper mapper,
        IAccessService accessService,
        ILogger<GetCourseByIdQueryHandle> logger,
        IAppSettings appSettings, UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.accessService = accessService;
        this.logger = logger;
        this.appSettings = appSettings;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<CourseDetailDto> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Try to get course with id {CourseId}", query.CourseId);
        if (!await accessService.HasAccessToCourse(query.CourseId, cancellationToken))
        {
            logger.LogWarning("User don't have access to course with id {CourseId}",
                query.CourseId);
            throw new ForbiddenException("You don't have access to this course.");
        }

        var course =
            await dbContext.Courses
                .Include(c => c.Creator)
                .Include(c => c.GradeCompositions
                    .Where(e => e.IsDeleted == false)
                    .OrderBy(e => e.Order))
                .ThenInclude(gc => gc.Grades)
                .ThenInclude(g => g.Request)
                .ThenInclude(r => r.Comments)
                .Include(c => c.StudentInfos)
                .ThenInclude(si => si.Student)
                .ThenInclude(s => s.User)
                .Include(c => c.Students).ThenInclude(cs => cs.Student)
                .Include(c => c.Teachers).ThenInclude(ct => ct.Teacher)
                .GetAsync(c => c.Id == query.CourseId, cancellationToken);

        logger.LogInformation("Course with id {CourseId} was found", query.CourseId);
        var result = mapper.Map<CourseDetailDto>(course);

        var st = course.Students.ToDictionary(student => student.StudentId,
            student => userManager.GetRolesAsync(student.Student).Result.FirstOrDefault() ?? "Student");
        var tc = course.Teachers.ToDictionary(teacher => teacher.TeacherId,
            teacher => userManager.GetRolesAsync(teacher.Teacher).Result.FirstOrDefault() ?? "Teacher");

        var requests = course.GradeCompositions
            .SelectMany(gc => gc.Grades)
            .Select(g => g.Request)
            .Where(r => r != null)
            .ToList();

        var listStudent = course.StudentInfos.Select(e => new
        {
            e.StudentId, UserId = e.Student.User?.Id ?? null, e.Name
        }).ToList();


        result.Requests = requests.Select(e =>
        {
            return mapper.Map<Domain.Request.Request, RequestDto>(e, opt =>
            {
                opt.AfterMap((src, des) =>
                {
                    des.GradeName = course.GradeCompositions
                        .FirstOrDefault(gc => gc.Grades.Any(g => g.Id == src.GradeId))
                        ?.Name ?? string.Empty;
                });
            });
        }).ToList();
        foreach (var requestDto in result.Requests)
        {
            foreach (var cmt in requestDto.Comments)
            {
                if (cmt.IsTeacher.Equals(true))
                {
                    cmt.Name = result.Teachers.FirstOrDefault(t => t.Id == cmt.UserId)?.FullName ?? "";
                }
                else
                {
                    var std = course.StudentInfos.FirstOrDefault(s =>
                        s.Student != null && s.Student.User != null && s.Student.User.Id == cmt.UserId);
                    var stdName = std?.Name;
                    var usrName = result.Students.FirstOrDefault(t => t.Id == cmt.UserId)?.FullName;
                    if (stdName != null)
                    {
                        cmt.Name = stdName;
                    }
                    else if (usrName != null)
                    {
                        cmt.Name = usrName;
                    }
                    else
                    {
                        cmt.Name = "";
                    }
                }
            }
        }


        foreach (var resultRequest in result.Requests)
        {
            resultRequest.StudentName = listStudent
                .Where(e => e.UserId == resultRequest.StudentId)
                .Select(e => e.Name)
                .FirstOrDefault() ?? "";
        }

        result.CreatorFullName = course.Creator.FullName;

        // TODO: Need url from frontend
        result.InviteLink = $"{appSettings.FrontendUrl}/course/invite/{course.InviteCode}";

        foreach (var stud in result.Students)
        {
            stud.Role = st.GetValueOrDefault(stud.Id);
        }

        foreach (var teacher in result.Teachers)
        {
            teacher.Role = tc.GetValueOrDefault(teacher.Id);
        }

        return result;
    }
}
