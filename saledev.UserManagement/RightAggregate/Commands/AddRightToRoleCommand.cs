using FluentValidation;
using MediatR;
using saledev.Result;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;

public class AddRightsToRoleCommand : IRequest<Result<Role>>
{
    public Guid RoleId { get; set; }
    public List<string>? Rights { get; set; }
    
    public class AddRightsToRoleCommandHandler : IRequestHandler<AddRightsToRoleCommand, Result<Role>>
    {
        private readonly IRepository<Right> repository;
        private readonly IRepository<Role> roleRepository;
        private readonly IValidator<Right> validator;

        public AddRightsToRoleCommandHandler(IRepository<Right> repository, IRepository<Role> roleRepository, IValidator<Right> validator)
        {
            this.repository = repository;
            this.roleRepository = roleRepository;
            this.validator = validator;
        }
        public async Task<Result<Role>> Handle(AddRightsToRoleCommand command, CancellationToken cancellationToken)
        {
            var rights = await repository.ListAsync(new RightByIdsSpec(command.Rights ?? new List<string>()), cancellationToken);

            if (!rights.Any())
            {
                throw new ApplicationException("No rights to add.");
            }

            var role = await roleRepository.GetByIdAsync(command.RoleId);
            if(role == null)
            {
                throw new ApplicationException("Role not found.");
            }

            foreach (var right in rights)
            {
                role.Rights.Add(right);
            }

            await roleRepository.UpdateAsync(role, cancellationToken);
            await roleRepository.SaveChangesAsync(cancellationToken);

            return new Result<Role>(role);
        }
    }
}
