namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentListAllTemplate;

/// <summary>
///
/// </summary>
public class GenerateStudentListAllTemplateResult
{
    /// <summary>
    /// Stream hold the file.
    /// </summary>
    public Stream FileContent { get; set; }

    /// <summary>
    /// Mimetype of template file.
    /// </summary>
    public string Mimetype { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// Name of file.
    /// </summary>
    public string FileName { get; set; }
}
