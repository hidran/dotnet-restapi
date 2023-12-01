using AutoMapper;
using PmsApi.DTO;
using PmsApi.Models;

namespace PmsApi.Utilities;
class UserProfile : Profile
{
    public UserProfile()
    {

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<User, UserDto>();
    }
}