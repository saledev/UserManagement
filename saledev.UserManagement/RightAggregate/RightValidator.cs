using FluentValidation;

namespace saledev.UserManagement;

public class RightValidator : AbstractValidator<Right>
{
    public RightValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(255);
    }
}
