using System.Runtime.CompilerServices;
using AutoMapper;
using PmsApi.DTO;
using PmsApi.Models;
using Task = PmsApi.Models.Task;

namespace PmsApi.Utilities;
class UserProfile : Profile
{
    public UserProfile()
    {

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        CreateMap<User, UserDto>()
        .ForMember(d => d.Projects, opt => opt.MapFrom(src => src.Projects))
        .ForMember(d => d.Tasks, opt => opt.MapFrom(src => src.Tasks)); ;
        CreateMap<Project, ProjectDto>();
        CreateMap<Task, TaskDto>();


    }
}