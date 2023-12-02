namespace PmsApi.DTO;


public record TaskDto(
     int TaskId,

     string Title,

     string? Description,

     int PriorityId,


     int StatusId,

     DateOnly DueDate,
     DateOnly CreatedDate,
     int ProjectId,

     int AssignedUserId




 );
