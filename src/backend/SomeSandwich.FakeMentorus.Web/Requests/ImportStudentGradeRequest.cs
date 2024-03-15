using System.ComponentModel.DataAnnotations;

namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Request hold student grade template file.
/// </summary>
public class ImportStudentGradeRequest
{
    /// <summary>
    /// Student grade template file.
    /// </summary>
    [Required]
    required public IFormFile File { get; set; }
}
