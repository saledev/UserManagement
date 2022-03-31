using Microsoft.Extensions.Localization;
using saledev.Authentication;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;

public class RightValidationService : IRightValidationService
{
    private readonly IReadRepository<Right> rightRepository;
    private readonly IReadRepository<User> userRepository;
    private readonly ISessionManager sessionManager;
    private readonly IStringLocalizer<RightValidationService> localizer;

    public RightValidationService(IReadRepository<Right> rightRepository, IReadRepository<User> userRepository,
        IStringLocalizer<RightValidationService> localizer, ISessionManager sessionManager)
    {
        this.rightRepository = rightRepository;
        this.userRepository = userRepository;
        this.localizer = localizer;
        this.sessionManager = sessionManager;
    }

    public async Task<bool> HasRight(string rightName)
    {
        var userId = sessionManager.GetUserId();
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null || !user.Roles.Any())
        {
            return false;
        }

        var right = rightRepository.GetByIdAsync(new RightId(rightName));
        if (right == null)
        {
            throw new ArgumentNullException("Right not found.");
        }

        bool hasRight = false;
        foreach(var role in user.Roles)
        {
            if(role.Rights.Any(x => x.Id == new RightId(rightName)))
            {
                hasRight = true;
            }
        }

        return hasRight;
    }
}
