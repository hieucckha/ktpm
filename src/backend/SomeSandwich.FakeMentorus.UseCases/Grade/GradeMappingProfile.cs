using AutoMapper;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;
using SomeSandwich.FakeMentorus.UseCases.Grade.GetGradeByStudentId;

namespace SomeSandwich.FakeMentorus.UseCases.Grade;

/// <summary>
/// Grade mapping profile.
/// </summary>
public class GradeMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GradeMappingProfile"/> class.
    /// </summary>
    public GradeMappingProfile()
    {
        CreateMap<Domain.Grade.Grade, GradeDto>();
        CreateMap<Domain.Grade.Grade, GradeCellDto>();

        CreateMap<User, UserWithoutStudentDto>()
            .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(des => des.Name, opt => opt.MapFrom(src => src.FullName));


        CreateMap<Domain.Grade.Grade, GradeDetailByStudentIdDto>();

    }
}
