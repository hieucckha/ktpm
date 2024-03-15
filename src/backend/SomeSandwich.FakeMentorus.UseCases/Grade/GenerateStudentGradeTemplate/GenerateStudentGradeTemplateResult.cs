namespace SomeSandwich.FakeMentorus.UseCases.Grade.GenerateStudentGradeTemplate;

/// <summary>
/// The result hold the student grade template file.
/// </summary>
public class GenerateStudentGradeTemplateResult
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
