using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;
using SomeSandwich.FakeMentorus.UseCases.Grade.CreateGrade;
using SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentGradeTemplate;
using SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentListTemplate;
using SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseId;
using SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseIdExport;
using SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByStudentId;
using SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByUserId;
using SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentGrade;
using SomeSandwich.FakeMentorus.UseCases.Grade.ImportStudentList;
using SomeSandwich.FakeMentorus.Web.Requests;

namespace SomeSandwich.FakeMentorus.Web.Controllers;

/// <summary>
/// Authentication controller.
/// </summary>
[ApiController]
[Route("api/grade")]
[ApiExplorerSettings(GroupName = "grade")]
[Authorize]
public class GradeController
{
    private readonly IMediator mediator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mediator">Mediator instance.</param>
    public GradeController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Generate student list template for import student to course.
    /// </summary>
    /// <param name="command">Generate student list template query.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns></returns>
    [HttpGet("student/tempalte")]
    public async Task<ActionResult> GenerateStudentListTemplate(
        [FromQuery] GenerateStudentListTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        result.FileContent.Position = 0;

        return new FileStreamResult(result.FileContent, result.Mimetype) { FileDownloadName = result.FileName };
    }

    /// <summary>
    /// Imports a student list for a specific course from a template.
    /// </summary>
    /// <param name="id">Id of course.</param>
    /// <param name="request">Request that hold the sheet of student list.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [HttpPost("student/template/{id:int}/import")]
    public async Task ImportStudentList([FromRoute] int id,
        [FromForm] ImportStudentListRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ImportStudentListCommand { CourseId = id, FileContent = request.File.OpenReadStream() };

        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Generate student grade template for import student grade.
    /// </summary>
    /// <param name="command">Command generate student grade template.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [HttpGet("template")]
    public async Task<ActionResult> GenerateStudentGradeTemplate(
        [FromQuery] GenerateStudentGradeTemplateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        result.FileContent.Position = 0;

        return new FileStreamResult(result.FileContent, result.Mimetype) { FileDownloadName = result.FileName };
    }

    /// <summary>
    /// Import student grade from template.
    /// </summary>
    /// <param name="id">Id of course.</param>
    /// <param name="request">Request that hold the sheet of student grade.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    [HttpPost("template/{id:int}/import")]
    public async Task ImportStudentGrade([FromRoute] int id,
        [FromForm] ImportStudentGradeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ImportStudentGradeCommand { CourseId = id, FileContent = request.File.OpenReadStream() };

        await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Get all grade by course id.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns></returns>
    [HttpGet("all")]
    public async Task<ActionResult<GetAllGradeByCourseIdResult>> GetAllGradeByCourseId(
        [FromQuery] GetAllGradeByCourseIdCommand command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Get all grade by course id and export to excel file.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns></returns>
    [HttpGet("all/export")]
    public async Task<ActionResult> GetAllGradeByCourseIdExport(
        [FromQuery] GetAllGradeByCourseIdExportQuery query,
        CancellationToken cancellationToken)
    {
        await mediator.Send(query, cancellationToken);

        var result = await mediator.Send(query, cancellationToken);
        result.FileContent.Position = 0;

        return new FileStreamResult(result.FileContent, result.Mimetype) { FileDownloadName = result.FileName };
    }

    /// <summary>
    /// Get grade by student id in a course.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="studentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    // [HttpGet("course/{courseId:int}/student/{studentId:int}")]
    // public async Task<ActionResult<GetGradeByStudentIdDto>> GetAllGradeByStudentId([FromRoute] int courseId,
    //     [FromRoute] string studentId, CancellationToken cancellationToken)
    // {
    //     var query = new GetGradeByStudentIdQuery() { CourseId = courseId, StudentId = studentId };
    //     return await mediator.Send(query, cancellationToken);
    // }
    [HttpGet("course/{courseId:int}/student/{studentId:int}")]
    public async Task<ActionResult<GetGradeByUserIdResult>> GetAllGradeByStudentId(
        [FromRoute] int courseId,
        [FromRoute] string studentId, CancellationToken cancellationToken)
    {
        var command = new GetGradeByUserIdQuery() { CourseId = courseId, StudentId = studentId };
        return await mediator.Send(command, cancellationToken);
    }

    /// <summary>
    /// Add or update grade.(Input grade for a student at a specific assignment).
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<GradeDto> AddGrade([FromBody] CreateGradeCommand command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
}
