using Ardalis.Specification;

namespace saledev.UserManagement;

public class UserByUsernameOrEmailSpec : Specification<User>, ISingleResultSpecification
{
    public UserByUsernameOrEmailSpec(string s)
    {
        Query.Where(x => x.Username == s || x.Email == s);
    }
}
