using Ardalis.Specification;

namespace saledev.UserManagement;

public class RightByIdsSpec : Specification<Right>
{
    public RightByIdsSpec(List<string> rightIds)
    {
        var rightIdsList = new List<RightId>();
        foreach (string rightId in rightIds)
        {
            rightIdsList.Add(new RightId(rightId));
        }

        Query.Where(x => rightIdsList.Contains(x.Id));
    }
}
