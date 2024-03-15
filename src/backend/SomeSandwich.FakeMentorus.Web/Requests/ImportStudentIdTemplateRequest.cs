using System.ComponentModel.DataAnnotations;

namespace SomeSandwich.FakeMentorus.Web.Requests;

/// <summary>
/// 
/// </summary>
public class ImportStudentIdTemplateRequest
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    required public IFormFile File { get; set; }
}
