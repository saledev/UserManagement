using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace saledev.UserManagement;

public static class StartupSetup
{
    public static void AddUserManagement(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(User).Assembly);
        services.AddMediatR(typeof(User).Assembly);
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddTransient<IUserService, UserService>();
    }

}
