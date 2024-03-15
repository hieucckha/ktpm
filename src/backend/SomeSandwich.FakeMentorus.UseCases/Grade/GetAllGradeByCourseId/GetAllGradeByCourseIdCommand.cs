using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseId;

/// <summary>
/// 
/// </summary>
public class GetAllGradeByCourseIdCommand : IRequest<GetAllGradeByCourseIdResult>
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    required public int CourseId { get; set; }
}
