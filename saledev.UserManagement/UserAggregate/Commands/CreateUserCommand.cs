using saledev.Result;
using MediatR;
using saledev.SharedKernel.Interfaces;
using FluentValidation;
using saledev.Result.FluentValidation;

namespace saledev.UserManagement;

public class CreateUserCommand : IRequest<Result<Guid>>
{
    public string Email { get; set; } = string.Empty;
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IRepository<User> repository;
        private readonly IValidator<User> validator;

        public CreateUserCommandHandler(IRepository<User> repository, IValidator<User> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }
        public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = command.Email
            };

            var validation = await validator.ValidateAsync(user);
            if (!validation.IsValid)
            {
                return Result<Guid>.Invalid(validation.AsErrors());
            }

            if (user.Email == "abc@abc.de")
            {
                throw new InvalidOperationException("something went wrong.");
            }

            if (user.Email == "abc2@abc.de")
            {
                throw new ApplicationException("something went wrong2.");
            }

            await repository.AddAsync(user);
            await repository.SaveChangesAsync();

            return new Result<Guid>(user.Id);
        }
    }
}
