namespace PmsApi.DTO;

public record UserDto
(
     int UserId,
    string UserName,
    string FirstName,
    string LastName,

    string Email,
    int RoleId,
    List<TaskDto> Tasks,
    List<ProjectDto> Projects
);