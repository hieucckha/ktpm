using AutoMapper;
using SomeSandwich.FakeMentorus.Domain.Request;
using SomeSandwich.FakeMentorus.UseCases.Comment.Common;

namespace SomeSandwich.FakeMentorus.UseCases.Comment;

/// <summary>
/// Comment request mapping profile.
/// </summary>
public class CommentRequestMappingProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public CommentRequestMappingProfile()
    {
        CreateMap<CommentRequest, CommentDto>();
    }
}
