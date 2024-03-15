namespace SomeSandwich.FakeMentorus.UseCases.Common.Service;

/// <summary>
/// Access service.
/// </summary>
public interface IAccessService
{
    /// <summary>
    ///Checks if user has access to course.
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> HasAccessToCourse(int courseId, CancellationToken cancellationToken);
}
