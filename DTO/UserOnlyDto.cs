namespace PmsApi.DTO;

public record UserOnlyDto(
    int UserId,
    string UserName,
    string FirstName,
    string LastName,

    string Email,
    int RoleId);