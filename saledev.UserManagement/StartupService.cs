using Microsoft.Extensions.DependencyInjection;

namespace saledev.UserManagement;

public static class StartupSetup
{
    public static void AddUserManagement(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
    }

}
