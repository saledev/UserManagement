using saledev.Result;
using MediatR;
using saledev.SharedKernel.Interfaces;
using FluentValidation;
using saledev.Result.FluentValidation;

namespace saledev.UserManagement;

public class SignupUserCommand : IRequest<Result<Guid>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public class SignupUserCommandHandler : IRequestHandler<SignupUserCommand, Result<Guid>>
    {
        private readonly IRepository<User> repository;
        private readonly IRepository<Role> roleRepository;
        private readonly IValidator<User> validator;

        public SignupUserCommandHandler(IRepository<User> repository, IRepository<Role> roleRepository, IValidator<User> validator)
        {
            this.repository = repository;
            this.roleRepository = roleRepository;
            this.validator = validator;
        }
        public async Task<Result<Guid>> Handle(SignupUserCommand command, CancellationToken cancellationToken)
        {
            var addEntity = new User
            {
                Username = command.Username,
                Email = command.Email
            };

            var roles = await roleRepository.ListAsync(cancellationToken);
            var defaultRole = roles.FirstOrDefault(x => x.IsDefaultRole);
            if(defaultRole == null)
            {
                throw new Exception("No default role specified.");
            }

            addEntity.Roles = new List<Role> { defaultRole };

            var validation = await validator.ValidateAsync(addEntity);
            if (!validation.IsValid)
            {
                return Result<Guid>.Invalid(validation.AsErrors());
            }

            await repository.AddAsync(addEntity, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            return new Result<Guid>(addEntity.Id);
        }
    }
}
