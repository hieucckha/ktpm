using System.ComponentModel.DataAnnotations;

namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// Request hold student list template file.
/// </summary>
public class ImportStudentListRequest
{
    /// <summary>
    /// Student list template file.
    /// </summary>
    [Required]
    required public IFormFile File { get; set; }
}
