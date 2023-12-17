

using Microsoft.AspNetCore.Identity;

namespace PmsApi.Models;

public class User : IdentityUser
{

    public string? FirstName { get; set; } = String.Empty;

    public string? LastName { get; set; } = String.Empty;

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
