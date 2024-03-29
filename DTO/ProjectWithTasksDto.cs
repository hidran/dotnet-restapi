using PmsApi.Models;

namespace PmsApi.DTO;

public record ProjectWithTasksDto(
 int ProjectId,
     string ProjectName,

     string? Description,

     DateOnly StartDate,

     DateOnly EndDate,

     int CategoryId,

     string ManagerId,
     ICollection<TaskDto> Tasks,
    UserOnlyDto Manager,
    CategoryDto Category
);