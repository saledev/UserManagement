using saledev.Result;
using MediatR;
using saledev.SharedKernel.Interfaces;
using saledev.Authentication;

namespace saledev.UserManagement;

public class CreateClaimCommand : IRequest<Result<string>>
{
    public Guid UserId { get; set; }
    public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, Result<string>>
    {
        private readonly IRepository<User> repository;
        private readonly IAuthenticationService authenticationService;

        public CreateClaimCommandHandler(IRepository<User> repository, IAuthenticationService authenticationService)
        {
            this.repository = repository;
            this.authenticationService = authenticationService;
        }
        public async Task<Result<string>> Handle(CreateClaimCommand command, CancellationToken cancellationToken)
        {
            var foundUser = await repository.GetByIdAsync(command.UserId, cancellationToken);

            if (foundUser == null)
            {
                throw new ApplicationException("User not found.");
            }

            var claim = new Claim()
            {
                UserId = foundUser.Id.ToString(),
                Roles = foundUser.Roles.Select(x => x.Id.ToString()).ToList(),
                Rights = new List<string>()
            };

            foreach (var role in foundUser.Roles)
            {
                var rightsAsString = role.Rights.Select(x => x.Id.ToString()).ToList();

                claim.Rights.AddRange(rightsAsString);
            }

            return new Result<string>(authenticationService.EncodeClaim(claim));
        }
    }
}
