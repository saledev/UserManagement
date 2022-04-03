using MediatR;
using saledev.Result;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;
public class DeleteRoleFromUserCommand : IRequest<Result<User>>
{
    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    public class DeleteRoleFromUserCommandHandler : IRequestHandler<DeleteRoleFromUserCommand, Result<User>>
    {
        private readonly IRepository<Role> repository;
        private readonly IRepository<User> userRepository;

        public DeleteRoleFromUserCommandHandler(IRepository<Role> repository, IRepository<User> userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }
        public async Task<Result<User>> Handle(DeleteRoleFromUserCommand command, CancellationToken cancellationToken)
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

            user.Roles.Remove(role);

            await userRepository.UpdateAsync(user, cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);

            return new Result<User>(user);
        }
    }
}
