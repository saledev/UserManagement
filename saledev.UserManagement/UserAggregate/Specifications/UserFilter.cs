using saledev.SharedKernel;

namespace saledev.UserManagement;

public class UserFilter : BaseFilter
{
    public string Email { get; set; } = string.Empty;
}
