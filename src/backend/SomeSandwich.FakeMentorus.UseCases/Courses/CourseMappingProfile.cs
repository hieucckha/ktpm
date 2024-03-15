using AutoMapper;
using SomeSandwich.FakeMentorus.Domain.Course;
using SomeSandwich.FakeMentorus.UseCases.Courses.Common;
using SomeSandwich.FakeMentorus.UseCases.Courses.GetCourseById;
using SomeSandwich.FakeMentorus.UseCases.Users.Common.Dtos;

namespace SomeSandwich.FakeMentorus.UseCases.Courses;

/// <summary>
/// Course mapping profile.
/// </summary>
public class CourseMappingProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CourseMappingProfile()
    {
        CreateMap<Course, CourseDto>()
            .AfterMap((src, des) =>
            {
                des.NumberOfStudents = src.Students.Count;
                des.NumberOfTeachers = src.Teachers.Count;
            });
        CreateMap<Course, CourseDetailDto>().AfterMap((src, des) =>
        {
            des.NumberOfStudents = src.Students.Count;
            des.NumberOfTeachers = src.Teachers.Count;
            des.Students = src.Students.Select(
                x => new UserCourseDto { Id = x.StudentId, FullName = x.Student.FullName, Role = null, }
            ).ToList();
            des.Teachers = src.Teachers.Select(
                x => new UserCourseDto { Id = x.TeacherId, FullName = x.Teacher.FullName, Role = null, }
            ).ToList();
            des.CreatorFullName = src.Creator.FullName;
        });

        CreateMap<CourseStudent, UserCourseDto>().AfterMap((src, des) =>
        {
            des.Id = src.StudentId;
            des.FullName = src.Student.FullName;
        });
        CreateMap<CourseTeacher, UserCourseDto>().AfterMap((src, des) =>
        {
            des.Id = src.TeacherId;
            des.FullName = src.Teacher.FullName;
        });
    }
}
