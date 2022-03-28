using Ardalis.Specification;
using saledev.SharedKernel.Helpers;

namespace saledev.UserManagement;

public class UserSpec : UserBaseSpec
{
    public UserSpec(UserFilter filter) : base(filter)
    {
        Query.OrderBy(x => x.Email)
                .ThenByDescending(x => x.Email);

        //if (filter.LoadChildren)
        //    Query.Include(x => x.Stores);

        if (filter.IsPagingEnabled)
            Query.Skip(PaginationHelper.CalculateSkip(filter))
                 .Take(PaginationHelper.CalculateTake(filter));
    }
}
