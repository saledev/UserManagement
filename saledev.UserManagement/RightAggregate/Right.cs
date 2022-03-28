using saledev.SharedKernel;
using saledev.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace saledev.UserManagement;

public class Right : BaseEntity<RightId>, IAggregateRoot
{
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
}
