using AutoMapper;
using SomeSandwich.FakeMentorus.UseCases.Grade.Common;
using SomeSandwich.FakeMentorus.UseCases.GradeComposition.Common;
using SomeSandwich.FakeMentorus.UseCases.GradeComposition.GetGradeCompositionById;

namespace SomeSandwich.FakeMentorus.UseCases.GradeComposition;

/// <summary>
/// Grade composition mapping profile.
/// </summary>
public class GradeCompositionMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GradeCompositionMappingProfile"/> class.
    /// </summary>
    public GradeCompositionMappingProfile()
    {
        CreateMap<Domain.Grade.GradeComposition, GradeCompositionDto>();
        CreateMap<Domain.Grade.GradeComposition, GradeCompositionDetailDto>();
    }
}
