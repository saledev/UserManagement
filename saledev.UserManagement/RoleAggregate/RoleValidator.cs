using FluentValidation;

namespace saledev.UserManagement;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(255);
    }
}
