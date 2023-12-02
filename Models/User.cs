

namespace PmsApi.Models;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = String.Empty;

    public string? FirstName { get; set; } = String.Empty;

    public string? LastName { get; set; } = String.Empty;

    public string Password { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;

    public int RoleId { get; set; } = 0;

    public ICollection<Project> Projects { get; set; } = new List<Project>();

    public Role? Role { get; set; }

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
