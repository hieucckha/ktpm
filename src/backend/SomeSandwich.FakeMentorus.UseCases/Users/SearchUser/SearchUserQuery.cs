using MediatR;
using Saritasa.Tools.Common.Pagination;
using SomeSandwich.FakeMentorus.UseCases.Common.Pagination;
using SomeSandwich.FakeMentorus.UseCases.Users.Common.Dtos;

namespace SomeSandwich.FakeMentorus.UseCases.Users.SearchUser;

/// <summary>
/// Search user query.
/// </summary>
public class SearchUserQuery : PageQueryFilter, IRequest<PagedList<UserDto>>
{
}
