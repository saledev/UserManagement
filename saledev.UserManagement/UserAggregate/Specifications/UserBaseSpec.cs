using Ardalis.Specification;

namespace saledev.UserManagement;

public class UserBaseSpec : Specification<User>
{
    public UserBaseSpec(UserFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.Email))
            Query.Where(x => x.Email == filter.Email);

        if (!string.IsNullOrEmpty(filter.SearchFor))
        {
            Query.Search(x => x.Email, "%" + filter.SearchFor + "%");
            //Query.Search(x => x.Username, "%" + filter.SearchFor + "%");
        }
    }
}
