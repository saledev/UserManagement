using FluentValidation;

namespace saledev.UserManagement;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();

        RuleFor(x => x.Username).MaximumLength(100);
    }
}
