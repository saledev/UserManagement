using MediatR;
using saledev.SharedKernel.Interfaces;

namespace saledev.UserManagement;

public class UserManagementDataSeeder : IUserManagementDataSeeder, IDataSeeder
{
    private readonly IMediator mediator;

    public UserManagementDataSeeder(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task SeedData()
    {
        var rightsForAdmin = new List<string>()
        {
            "User.Create",
            "User.Update",
            "User.Delete",
            "User.Read",

            "Right.Create",
            "Right.Update",
            "Right.Delete",
            "Right.Read",

            "Role.Create",
            "Role.Update",
            "Role.Delete",
            "Role.Read",
        };

        foreach (var right in rightsForAdmin)
        {
            await mediator.Send(new CreateRightCommand()
            {
                Id = right,
                Title = right.Replace(".", " - ")
            });
        }

        var adminRoleId = (await mediator.Send(new CreateRoleCommand()
        {
            Title = "Admin",
            RoleType = RoleType.Owner,
            IsDefaultRole = false,
            Rights = rightsForAdmin
        })).Value;

        var rightsForMember = new List<string>()
        {
            "User.SelfManagement"
        };

        foreach (var right in rightsForMember)
        {
            await mediator.Send(new CreateRightCommand()
            {
                Id = right,
                Title = right.Replace(".", " - ")
            });
        }

        var memberRoleId = (await mediator.Send(new CreateRoleCommand()
        {
            Title = "Member",
            IsDefaultRole = true,
            RoleType = RoleType.Customer,
            Rights = rightsForMember
        })).Value;

        await mediator.Send(new CreateUserCommand()
        {
            Username = "admin",
            Email = "admin@admin.de",
            RoleIds = new List<Guid>() { adminRoleId, memberRoleId }
        });
    }
}