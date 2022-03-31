using saledev.SharedKernel;
using saledev.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace saledev.UserManagement;

public class Role : BaseEntity<Guid>, IAggregateRoot
{
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;

    public bool IsDefaultRole { get; set; }

    public List<Right> Rights { get; set; } = null!;
}
