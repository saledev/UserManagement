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
        var rightsForAdmin = new List<string>();

        rightsForAdmin.Add((await mediator.Send(new CreateRightCommand()
        {
            Id = "User.Create",
            Title = "User - Create"
        })).Value.ToString());

        var adminRoleId = (await mediator.Send(new CreateRoleCommand()
        {
            Title = "Admin",
            IsDefaultRole = false,
            Rights = rightsForAdmin
        })).Value;

        var rightsForMember = new List<string>();

        rightsForMember.Add((await mediator.Send(new CreateRightCommand()
        {
            Id = "User.Self",
            Title = "User - Self-Management"
        })).Value.ToString());

        var memberRoleId = (await mediator.Send(new CreateRoleCommand()
        {
            Title = "Member",
            IsDefaultRole = false,
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