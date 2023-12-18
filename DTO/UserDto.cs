namespace PmsApi.DTO;

public record UserDto
(
     string Id,
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNUmber,
    string Email,
    List<TaskDto> Tasks,
    List<ProjectDto> Projects
);