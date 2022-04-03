using MediatR;
using saledev.Result;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;
public class AddRoleToUserCommand : IRequest<Result<User>>
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, Result<User>>
    {
        private readonly IRepository<Role> repository;
        private readonly IRepository<User> userRepository;

        public AddRoleToUserCommandHandler(IRepository<Role> repository, IRepository<User> userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }
        public async Task<Result<User>> Handle(AddRoleToUserCommand command, CancellationToken cancellationToken)
        {
            var role = await repository.GetByIdAsync(command.RoleId);
            if(role == null)
            {
                throw new ApplicationException("Role not found.");
            }

            var user = await userRepository.GetByIdAsync(command.UserId);
            if (user == null)
            {
                throw new ApplicationException("User not found.");
            }

            user.Roles.Add(role);

            await userRepository.UpdateAsync(user, cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);

            return new Result<User>(user);
        }
    }
}
