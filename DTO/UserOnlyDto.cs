namespace PmsApi.DTO;

public record UserOnlyDto(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email
    );