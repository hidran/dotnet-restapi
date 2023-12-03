
namespace PmsApi.Models;
public class ProjectCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = "";

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
