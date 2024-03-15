namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByStudentId;

/// <summary>
/// Grade by student id dto.
/// </summary>
public class GetGradeByStudentIdDto
{
    /// <summary>
    /// Student id.
    /// </summary>
    public string StudentId { get; set; } = string.Empty;

    /// <summary>
    /// Student name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Grade details.
    /// </summary>
    public List<GradeDetailByStudentIdDto> GradeDetails { get; set; } = new List<GradeDetailByStudentIdDto>();
}
