using System.Runtime.CompilerServices;
using AutoMapper;
using DTO;
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
        CreateMap<User, UserOnlyDto>();
        CreateMap<User, UserDto>()
        .ForMember(d => d.Projects, opt => opt.MapFrom(src => src.Projects))
        .ForMember(d => d.Tasks, opt => opt.MapFrom(src => src.Tasks)); ;
        CreateMap<Project, ProjectDto>();

        CreateMap<Priority, PriorityDto>();
        CreateMap<CreatePriorityDto, Priority>();

        CreateMap<Status, StatusDto>();
        CreateMap<CreateStatusDto, Status>();

        CreateMap<ProjectCategory, CategoryDto>();
        CreateMap<CreateCategoryDto, ProjectCategory>();

        CreateMap<CreateProjectDto, Project>();
        CreateMap<Project, ProjectWithTasksDto>()
         .ForMember(d => d.Manager, opt => opt.MapFrom(src => src.Manager));

        CreateMap<Task, TaskDto>();
        CreateMap<CreateTaskDto, Task>();

        CreateMap<Task, TaskAllDto>();


        CreateMap<TaskAttachment, AttachmentWithTaskDto>();
        //   .ForMember(d => d.Task, opt => opt.MapFrom(src => src.Task));
        CreateMap<ProjectCategory, CategoryDto>();


    }
}