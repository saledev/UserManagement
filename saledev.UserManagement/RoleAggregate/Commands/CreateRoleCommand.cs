using FluentValidation;
using MediatR;
using saledev.Result;
using saledev.Result.FluentValidation;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement.RoleAggregate.Commands;

public class CreateRoleCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; } = string.Empty;
    public bool IsDefaultRole { get; set; }
    public List<string>? Rights { get; set; }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<Guid>>
    {
        private readonly IRepository<Role> repository;
        private readonly IRepository<Right> rightRepository;
        private readonly IValidator<Role> validator;

        public CreateRoleCommandHandler(IRepository<Role> repository, IRepository<Right> rightRepository, IValidator<Role> validator)
        {
            this.repository = repository;
            this.rightRepository = rightRepository;
            this.validator = validator;
        }
        public async Task<Result<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var addEntity = new Role
            {
                Title = command.Title,
                IsDefaultRole = command.IsDefaultRole
            };

            if (command.Rights != null && command.Rights.Any())
            {
                var rightsOnDb = await rightRepository.ListAsync();
                var rights = new List<Right>();
                foreach (var right in command.Rights)
                {
                    var rightFound = rightsOnDb.FirstOrDefault(x => x.Id == new RightId(right));
                    if(rightFound != null)
                    {
                        rights.Add(rightFound);
                    }
                }

                addEntity.Rights = rights;
            }

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
