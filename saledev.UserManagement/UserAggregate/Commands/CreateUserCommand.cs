using saledev.Result;
using MediatR;
using saledev.SharedKernel.Interfaces;
using FluentValidation;
using saledev.Result.FluentValidation;

namespace saledev.UserManagement;

public class CreateUserCommand : IRequest<Result<Guid>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Guid>? RoleIds { get; set; }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IRepository<User> repository;
        private readonly IRepository<Role> roleRepository;
        private readonly IValidator<User> validator;

        public CreateUserCommandHandler(IRepository<User> repository, IRepository<Role> roleRepository, IValidator<User> validator)
        {
            this.repository = repository;
            this.roleRepository = roleRepository;
            this.validator = validator;
        }
        public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var addEntity = new User
            {
                Username = command.Username,
                Email = command.Email
            };

            if (command.RoleIds == null)
            {
                throw new ArgumentNullException("Please define at least one role for the new user.");
            }

            var rolesOnDb = await roleRepository.ListAsync();
            var roles = new List<Role>();
            foreach (var roleId in command.RoleIds)
            {
                var roleFound = rolesOnDb.FirstOrDefault(x => x.Id == roleId);
                if (roleFound != null)
                {
                    roles.Add(roleFound);
                }
            }
            addEntity.Roles = roles;

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
