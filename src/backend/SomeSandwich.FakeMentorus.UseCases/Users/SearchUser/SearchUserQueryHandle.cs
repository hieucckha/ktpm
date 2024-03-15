using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Domain.Exceptions;
using Saritasa.Tools.EntityFrameworkCore.Pagination;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.UseCases.Users.Common.Dtos;

namespace SomeSandwich.FakeMentorus.UseCases.Users.SearchUser;

/// <summary>
/// Handle for <see cref="SearchUserQuery"/>.
/// </summary>
public class SearchUserQueryHandle : IRequestHandler<SearchUserQuery, PagedList<UserDto>>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;
    private readonly ILoggedUserAccessor loggedUserAccessor;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
    /// <param name="loggedUserAccessor"></param>
    /// <param name="userManager"></param>
    public SearchUserQueryHandle(IAppDbContext dbContext, IMapper mapper, ILoggedUserAccessor loggedUserAccessor,
        UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.loggedUserAccessor = loggedUserAccessor;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<PagedList<UserDto>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = loggedUserAccessor.GetCurrentUserId();
        var user = await userManager.FindByIdAsync(currentUserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (role is not "Admin")
        {
            // throw new ForbiddenException("Only admin can search users");
        }

        var query = dbContext.Users
            .Include(e => e.Student)
            .AsQueryable();

        var pagedList =
            await EFPagedListFactory.FromSourceAsync(query, request.Page, request.PageSize, cancellationToken);
        var result = pagedList.Convert(c => mapper.Map<UserDto>(c));
        var queryResult = query.ToList();
        foreach (var userDto in result)
        {
            var something = queryResult.FirstOrDefault(e => e.Id == userDto.Id);
            var lockoutEnabled = something?.LockoutEnabled;
            var isTimeout = something?.LockoutEnd;
            userDto.Status = lockoutEnabled is true && isTimeout != null ? UserStatus.Banned : UserStatus.Active;

            var userResult = queryResult.FirstOrDefault(e => e.Id == userDto.Id);

            userDto.Role = userResult != null
                ? (await userManager.GetRolesAsync(userResult)).FirstOrDefault()
                : "Student";
        }

        return result;
    }
}
