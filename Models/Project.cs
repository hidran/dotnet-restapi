

namespace PmsApi.Models;

public class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = String.Empty;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int CategoryId { get; set; }

    public string ManagerId { get; set; } = String.Empty;

    public ProjectCategory? Category { get; set; }

    public User? Manager { get; set; }

    public ICollection<Task>? Tasks { get; set; } = new List<Task>();
}
