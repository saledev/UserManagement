using FluentValidation;
using MediatR;
using saledev.Result;
using saledev.Result.FluentValidation;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement.RightAggregate.Commands;

public class CreateRightCommand : IRequest<Result<RightId>>
{
    public string Title { get; set; } = string.Empty;
    public class CreateRightCommandHandler : IRequestHandler<CreateRightCommand, Result<RightId>>
    {
        private readonly IRepository<Right> repository;
        private readonly IValidator<Right> validator;

        public CreateRightCommandHandler(IRepository<Right> repository, IValidator<Right> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }
        public async Task<Result<RightId>> Handle(CreateRightCommand command, CancellationToken cancellationToken)
        {
            var addEntity = new Right
            {
                Title = command.Title
            };

            var validation = await validator.ValidateAsync(addEntity);
            if (!validation.IsValid)
            {
                return Result<RightId>.Invalid(validation.AsErrors());
            }

            await repository.AddAsync(addEntity, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            return new Result<RightId>(addEntity.Id);
        }
    }
}
