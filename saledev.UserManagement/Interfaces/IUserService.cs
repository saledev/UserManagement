namespace saledev.UserManagement;

public interface IUserService
{
    Task<User?> GetByGuid(Guid guid);
    Task<User> GetById(string id);
}
