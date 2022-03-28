using AutoMapper;

namespace saledev.UserManagement;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        //CreateMap<List<User>, List<UserDto>>();
    }
}

public record UserDto(Guid Id, string Email);
