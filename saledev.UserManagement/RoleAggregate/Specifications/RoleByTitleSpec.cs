using Ardalis.Specification;

namespace saledev.UserManagement;

public class RoleByTitleSpec : Specification<Role>, ISingleResultSpecification
{
    public RoleByTitleSpec(string title)
    {
        Query.Where(x => x.Title == title);
    }
}
