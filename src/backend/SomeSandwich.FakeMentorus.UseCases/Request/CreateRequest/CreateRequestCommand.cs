using MediatR;
using SomeSandwich.FakeMentorus.UseCases.Request.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Request.CreateRequest;

/// <summary>
/// Command to create a request of a student to a grade.
/// </summary>
public class CreateRequestCommand : IRequest<RequestDto>
{
    /// <summary>
    /// The id of grade to request.
    /// </summary>
    required public int GradeCompositionId { get; set; }

    /// <summary>
    /// The grade that the student expect to get.
    /// </summary>
    required public int ExceptedGrade { get; set; }

    /// <summary>
    /// Reason of the request.
    /// </summary>
    required public string Reason { get; set; }
}
