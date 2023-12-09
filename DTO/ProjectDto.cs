namespace PmsApi.DTO;

public record ProjectDto(
     int ProjectId,

     string ProjectName,

     string Description,

     DateOnly StartDate,

     DateOnly EndDate,

     int CategoryId,

     int ManagerId
);
