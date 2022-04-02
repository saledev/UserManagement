using saledev.SharedKernel;
using saledev.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace saledev.UserManagement;

public class Role : BaseEntity<Guid>, IAggregateRoot
{
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    public RoleType RoleType { get; set; }

    public bool IsDefaultRole { get; set; }

    public ICollection<Right> Rights { get; set; } = null!;
    public ICollection<User> Users { get; set; } = null!;
}
