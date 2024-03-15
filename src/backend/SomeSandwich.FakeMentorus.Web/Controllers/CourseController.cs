using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saritasa.Tools.Common.Pagination;
using SomeSandwich.FakeMentorus.UseCases.Courses.AssignByCode;
using SomeSandwich.FakeMentorus.UseCases.Courses.AssignByEmail;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;
using SomeSandwich.FakeMentorus.UseCases.Courses.CreateCourse;
using SomeSandwich.FakeMentorus.UseCases.Courses.CreateInvitationLinkByEmail;
using SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseById;
using SomeSandwich.FakeMentorus.UseCases.Courses.SearchCourse;
using SomeSandwich.FakeMentorus.UseCases.Courses.ToggleActivated;
using SomeSandwich.FakeMentorus.UseCases.Courses.UpdateCourse;
using SomeSandwich.FakeMentorus.Web.Requests;

namespace SomeSandwich.FakeMentorus.Web.Controllers;

/// <summary>
/// Course controller.
/// </summary>
[ApiController]
[Route("api/course")]
[ApiExplorerSettings(GroupName = "course")]
public class CourseController
{
    private readonly IMediator mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mediator"></param>
    public CourseController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Create new course.
    /// </summary>
    /// <param name="command">Create course command.</param>
    /// <param name="cancellationToken"></param>
    [HttpPost("")]
    [Authorize]
    public async Task<int> Create(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Search courses.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("query")]
    [Authorize]
    public async Task<PagedListMetadataDto<CourseDto>> SearchCourses(
        [FromQuery] SearchCoursesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return result.ToMetadataObject();
    }

    /// <summary>
    /// Get course by id.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{courseId:int}")]
    [Authorize]
    public async Task<CourseDetailDto> GetCourseById(
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCourseByIdQuery { CourseId = courseId }, cancellationToken);

        return result;
    }

    /// <summary>
    /// Update course.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    [HttpPatch("{courseId:int}")]
    public async Task Update(
        [FromRoute] int courseId,
        [FromBody] UpdateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCourseCommand
        {
            CourseId = courseId,
            Name = request.Name,
            Description = request.Description
        };
        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Create invitation link per user and send to email.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    [HttpPost("{courseId:int}/invite-email")]
    [Authorize]
    public async Task CreateInviteLinkByEmail(
        [FromRoute] int courseId,
        [FromBody] InviteByEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateInvitationLinkByEmailCommand { CourseId = courseId, Email = request.Email };
        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Join course by invitation link in email.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    [HttpPost("invite-email/confirm")]
    [Authorize]
    public async Task JoinCourseByEmail(
        [FromBody] JoinCourseByEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignByEmailCommand { Token = request.Token };
        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Join course by invite code (for anonymous users).
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("join")]
    [Authorize]
    public async Task JoinCourseByCode(
        [FromBody] JoinCourseByCodeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignByCodeCommand { InviteCode = request.InviteCode };

        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Toggle activate course.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="cancellationToken"></param>
    [HttpPost("{courseId:int}/activate")]
    [Authorize]
    public async Task ActivateCourse(
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        var command = new ToggleActivatedCommand { CourseId = courseId };
        await mediator.Send(command, cancellationToken);
    }
}
