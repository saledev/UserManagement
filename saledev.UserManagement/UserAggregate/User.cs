using saledev.SharedKernel;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;

public class User : BaseEntity<Guid>, IAggregateRoot
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool OptInPromotional { get; set; }
    public List<Role> Roles { get; set; } = null!;
}
