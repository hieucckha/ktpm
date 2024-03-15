using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Request.GetRequestById;

/// <summary>
/// Handler for <see cref="GetRequestByIdQuery"/>.
/// </summary>
public class GetRequestByIdQueryHandle : IRequestHandler<GetRequestByIdQuery, RequestDetailDto>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public GetRequestByIdQueryHandle(IAppDbContext context, IMapper mapper)
    {
        this.dbContext = context;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RequestDetailDto> Handle(GetRequestByIdQuery query, CancellationToken cancellationToken)
    {
        var request = await dbContext.Requests
            .Include(e => e.Student)
            .Include(e => e.Comments)
            .ThenInclude(c => c.User)
            .Where(e => e.Id == query.RequestId)
            .FirstOrDefaultAsync(cancellationToken);

        if (request == null)
        {
            throw new NotFoundException("Request not found");
        }

        var result = mapper.Map<RequestDetailDto>(request);
        result.StudentName = $"{request.Student.FullName}";

        foreach (var resultComment in result.Comments)
        {
            resultComment.Name = request.Comments.FirstOrDefault(e => e.Id == resultComment.Id)?.User.FullName ?? "";
        }

        return result;
    }
}
