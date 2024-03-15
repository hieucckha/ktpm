using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Saritasa.Tools.EntityFrameworkCore;
using SomeSandwich.FakeMentorus.Domain.Users;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.UseCases.Users.GetUserById;

/// <summary>
/// Handler for <see cref="GetUserByIdQuery" />.
/// </summary>
internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
    private readonly IAppDbContext dbContext;
    private readonly IMapper mapper;
    private readonly RoleManager<AppIdentityRole> roleManager;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">Database context.</param>
    /// <param name="mapper">Automapper instance.</param>
    /// <param name="roleManager">Role manager.</param>
    /// <param name="userManager"></param>
    public GetUserByIdQueryHandler(IAppDbContext dbContext, IMapper mapper, RoleManager<AppIdentityRole> roleManager,
        UserManager<User> userManager)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.GetAsync(u => u.Id == request.UserId, cancellationToken);
        var userRtn = mapper.Map<UserDetailsDto>(user);

        userRtn.Role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Student";


        return userRtn;
    }

    internal class GetUserByIdQueryMappingProfile : Profile
    {
        public GetUserByIdQueryMappingProfile() => CreateMap<User, UserDetailsDto>();
    }
}
