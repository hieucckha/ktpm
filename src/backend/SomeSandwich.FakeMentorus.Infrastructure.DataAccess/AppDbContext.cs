using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.Domain.Grade;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.Domain.Student;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.Infrastructure.DataAccess;

/// <summary>
///     Application unit of work.
/// </summary>
public class AppDbContext : IdentityDbContext<User, AppIdentityRole, int>, IAppDbContext, IDataProtectionKeyContext
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="Microsoft.EntityFrameworkCore.DbContext" />.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Courses.
    /// </summary>
    public DbSet<Course> Courses { get; private set; }

    /// <summary>
    /// Course students.
    /// </summary>
    public DbSet<CourseStudent> CourseStudents { get; private set; }

    /// <summary>
    /// Course teachers.
    /// </summary>
    public DbSet<CourseTeacher> CourseTeachers { get; private set; }

    /// <summary>
    /// Grade compositions.
    /// </summary>
    public DbSet<GradeComposition> GradeCompositions { get; private set; }

    /// <summary>
    /// Grades.
    /// </summary>
    public DbSet<Grade> Grades { get; private set; }

    /// <summary>
    /// Requests.
    /// </summary>
    public DbSet<Request> Requests { get; private set; }

    /// <summary>
    /// Comment requests.
    /// </summary>
    public DbSet<CommentRequest> CommentRequests { get; private set; }

    /// <summary>
    /// Students.
    /// </summary>
    public DbSet<Student> Students { get; private set; }

    /// <summary>
    /// Student infos.
    /// </summary>
    public DbSet<StudentInfo> StudentInfos { get; private set; }

    /// <inheritdoc />
    public DbSet<DataProtectionKey> DataProtectionKeys { get; private set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        RestrictCascadeDelete(modelBuilder);
        ForceHavingAllStringsAsVarchars(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private static void RestrictCascadeDelete(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static void ForceHavingAllStringsAsVarchars(ModelBuilder modelBuilder)
    {
        var stringColumns = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(string));
        foreach (var mutableProperty in stringColumns)
        {
            mutableProperty.SetIsUnicode(false);
        }
    }
}
