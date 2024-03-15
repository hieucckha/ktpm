using Microsoft.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.Domain.Grade;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;

namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Application abstraction for unit of work.
/// </summary>
public interface IAppDbContext : IDbContextWithSets, IDisposable
{
    /// <summary>
    /// Users.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Courses.
    /// </summary>
    DbSet<Course> Courses { get; }

    /// <summary>
    /// Course students.
    /// </summary>
    DbSet<CourseStudent> CourseStudents { get; }

    /// <summary>
    /// Course teachers.
    /// </summary>
    DbSet<CourseTeacher> CourseTeachers { get; }

    /// <summary>
    /// Grade compositions.
    /// </summary>
    DbSet<GradeComposition> GradeCompositions { get; }

    /// <summary>
    /// Grades.
    /// </summary>
    DbSet<Grade> Grades { get; }

    /// <summary>
    /// Requests.
    /// </summary>
    DbSet<Request> Requests { get; }

    /// <summary>
    /// Comment requests.
    /// </summary>
    DbSet<CommentRequest> CommentRequests { get; }

    DbSet<Student> Students { get; }

    DbSet<StudentInfo> StudentInfos { get; }
}
