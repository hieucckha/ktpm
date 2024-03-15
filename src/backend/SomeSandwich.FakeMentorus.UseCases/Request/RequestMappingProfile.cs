using AutoMapper;
using SomeSandwich.FakeMentorus.UseCases.Request.Common;
using SomeSandwich.FakeMentorus.UseCases.Request.CreateRequest;
using SomeSandwich.FakeMentorus.UseCases.Request.GetRequestById;

namespace SomeSandwich.FakeMentorus.UseCases.Request;

/// <summary>
///   Request mapping profile.
/// </summary>
public class RequestMappingProfile : Profile
{
    /// <summary>
    ///  Initializes a new instance of the <see cref="RequestMappingProfile"/> class.
    /// </summary>
    public RequestMappingProfile()
    {
        CreateMap<Domain.Request.Request, RequestDto>()
            .AfterMap((src, des) => { des.StudentName = src.Student.FirstName + " " + src.Student.LastName; });

        CreateMap<Domain.Request.Request, RequestDetailDto>();
    }
}
