using PmsApi.Models;

namespace PmsApi.DTO;


public record TaskAllDto(
     int TaskId,

     string Title,

     string Description,

     int PriorityId,


     int StatusId,

     DateOnly DueDate,

     DateOnly CreatedDate,

     int ProjectId,

     int AssignedUserId,
    UserOnlyDto AssignedUser,

    ProjectDto Project,
 List<TaskAttachmentDto> TaskAttachments




 );
