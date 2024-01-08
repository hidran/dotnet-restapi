using System.ComponentModel.DataAnnotations;

namespace PmsApi.DTO;

public class CreateUserDto
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
   
}
