using PmsApi.Models;

namespace PmsApi.DTO;

public record ProjectWithTasksDto(

     string ProjectName,

     string? Description,

     DateOnly StartDate,

     DateOnly EndDate,

     int CategoryId,

     int ManagerId,
     ICollection<TaskDto> Tasks,
    UserOnlyDto Manager,
    CategoryDto Category
);