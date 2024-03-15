using MediatR;

namespace SomeSandwich.FakeMentorus.UseCases.Request.GetRequestById;

public class GetRequestByIdQuery : IRequest<RequestDetailDto>
{
    public int RequestId { get; set; }
}
