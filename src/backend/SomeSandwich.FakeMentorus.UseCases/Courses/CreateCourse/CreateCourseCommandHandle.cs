using System.Text;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using shortid.Configuration;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Courses.CreateCourse;

/// <summary>
/// Handler for <see cref="CreateCourseCommand"/>.
/// </summary>
internal partial class CreateCourseCommandHandle : IRequestHandler<CreateCourseCommand, int>
{
    private readonly ILogger<CreateCourseCommandHandle> logger;
    private readonly IAppDbContext dbContext;
    private readonly Random rand = new Random();
    private readonly UserManager<User> userManager;
    private readonly ILoggedUserAccessor loggedUserAccessor;


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    /// <param name="userManager"></param>
    /// <param name="loggedUserAccessor"></param>
    public CreateCourseCommandHandle(ILogger<CreateCourseCommandHandle> logger,
        IAppDbContext dbContext, UserManager<User> userManager, ILoggedUserAccessor loggedUserAccessor)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.loggedUserAccessor = loggedUserAccessor;
    }

    /// <inheritdoc />
    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        {
            var userId = loggedUserAccessor.GetCurrentUserId();
            var user = await userManager.FindByIdAsync(userId.ToString());
            var role = await userManager.GetRolesAsync(user);
            if (role.Contains("Student"))
            {
                throw new Exception("Student can't create course");
            }

            if (role.Contains("Admin"))
            {
                throw new Exception("Admin can't create course");
            }

            logger.LogInformation($"Course creating...");
            var course = new Course
            {
                Name = request.Name,
                Description = request
                    .Description,
                InviteCode = GenerateInviteCode("AAAA-xxxx-####"),
                CreatorId = userId,
                ClassCode = GenerateInviteCode("Axxxxxxxxxx##")
            };

            var result = await dbContext.Courses.AddAsync(course, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            await dbContext.CourseTeachers.AddAsync(
                new CourseTeacher { CourseId = result.Entity.Id, TeacherId = userId }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);


            logger.LogInformation("Course {CourseId} created", course.Id);

            return result.Entity.Id;
        }
    }

    private string GenerateInviteCode(string format)
    {
        var regexItem = MyRegex();

        if (regexItem.IsMatch(format))
        {
            throw new Exception("Contains not allowed characters");
        }

        var number = new string("0123456789");
        var bigger = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        var smaller = new string("abcdefghijklmnopqrstuvwxyz");
        var special = number + bigger + smaller;
        var code = new StringBuilder();
        code.Append(GetRandomChar(bigger));
        foreach (var c in format)
        {
            switch (c)
            {
                case 'A':
                    code.Append(GetRandomChar(bigger));
                    break;
                case 'a':
                    code.Append(GetRandomChar(smaller));
                    break;
                case '#':
                    code.Append(GetRandomChar(number));
                    break;
                case 'x':
                    code.Append(GetRandomChar(special));
                    break;
                default:
                    code.Append(c);
                    break;
            }
        }

        return code.ToString();
    }

    private char GetRandomChar(string str)
    {
        return str[rand.Next(0, str.Length)];
    }

    [GeneratedRegex("^[b-zB-Z0-9!@#$%^&*()+=\\[\\]{};':\"\\\\|,.<>\\/?]*$")]
    private static partial Regex MyRegex();
}
