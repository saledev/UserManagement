using saledev.Result;
using MediatR;
using saledev.SharedKernel.Interfaces;
using FluentValidation;
using saledev.Result.FluentValidation;

namespace saledev.UserManagement;

public class UpdateUserCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<Guid>? RoleIds { get; set; }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<Guid>>
    {
        private readonly IRepository<User> repository;
        private readonly IRepository<Role> roleRepository;
        private readonly IValidator<User> validator;

        public UpdateUserCommandHandler(IRepository<User> repository, IRepository<Role> roleRepository, IValidator<User> validator)
        {
            this.repository = repository;
            this.roleRepository = roleRepository;
            this.validator = validator;
        }
        public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var updateEntity = await repository.GetByIdAsync(command.Id);
            if(updateEntity == null)
            {
                throw new ArgumentNullException(nameof(updateEntity));
            }

            updateEntity.Email = command.Email;

            if(command.RoleIds != null)
            {
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

                updateEntity.Roles = roles;
            }

            var validation = await validator.ValidateAsync(updateEntity);
            if (!validation.IsValid)
            {
                return Result<Guid>.Invalid(validation.AsErrors());
            }

            await repository.SaveChangesAsync();

            return new Result<Guid>(updateEntity.Id);
        }
    }
}
