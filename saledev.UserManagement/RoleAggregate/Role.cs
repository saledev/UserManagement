using saledev.SharedKernel;
using saledev.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace saledev.UserManagement;

public class Role : BaseEntity<int>, IAggregateRoot
{
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
}
