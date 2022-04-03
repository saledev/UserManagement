using Ardalis.Specification;

namespace saledev.UserManagement;

public class RightByIdsSpec : Specification<Right>
{
    public RightByIdsSpec(List<string> rightIds)
    {
        Query.Where(x => rightIds.Contains(x.Id.ToString()));
    }
}
