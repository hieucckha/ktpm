namespace SomeSandwich.FakeMentorus.UseCases.Grade.GetAllGradeByCourseIdExport;

/// <summary>
/// Result of get all grade by course id.
/// </summary>
public class GetAllGradeByCourseIdExportResult
{
    /// <summary>
    /// Stream hold the file.
    /// </summary>
    public Stream FileContent { get; set; }

    /// <summary>
    /// Mimetype of template file.
    /// </summary>
    public string Mimetype { get; set; } =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// Name of file.
    /// </summary>
    public string FileName { get; set; }
}
