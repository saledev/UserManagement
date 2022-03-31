using Microsoft.Extensions.Localization;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;

public class UserService : IUserService
{
    private readonly IReadRepository<User> repository;
    private readonly IStringLocalizer<UserService> localizer;

    public UserService(IReadRepository<User> repository, IStringLocalizer<UserService> localizer)
    {
        this.repository = repository;
        this.localizer = localizer;
    }

    public async Task<User?> GetByGuid(Guid guid)
    {
        return await repository.GetByIdAsync(guid);
    }

    public async Task<User> GetById(string id)
    {
        var user = await GetByGuid(Guid.Parse(id));
        if (user == null)
        {
            throw new ArgumentNullException(localizer["{0} not found by id {1}.", nameof(user), id]);
        }

        return user;
    }

    public async Task<User> GetByUsername(string username)
    {
        var user = await repository.GetBySpecAsync(new UserByUsernameOrEmailSpec(username));
        if (user == null)
        {
            throw new ArgumentNullException(localizer["{0} not found by username {1}.", nameof(user), username]);
        }

        return user;
    }
}
