using saledev.Result;
using MediatR;
using saledev.SharedKernel.Helpers;
using saledev.SharedKernel.Interfaces;
using AutoMapper;

namespace saledev.UserManagement;

public class GetAllUsersQuery : IRequest<PagedResult<List<UserDto>>>
{
    public UserFilter Filter { get; set; } = new UserFilter();

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<List<UserDto>>>
    {

        private readonly IReadRepository<User> repository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IReadRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<PagedResult<List<UserDto>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            query.Filter = PaginationHelper.ProvideDefaultValues(query.Filter);

            int totalRecords = await repository.CountAsync(new UserBaseSpec(query.Filter));

            var pagedInfo = new PagedInfo(query.Filter.Page, query.Filter.PageSize, totalRecords);

            var userList = await repository.ListAsync(new UserSpec(query.Filter));
            var userListDto = mapper.Map<List<UserDto>>(userList);

            return Result<List<UserDto>>.Success(userListDto).ToPagedResult(pagedInfo);
        }
    }
}
