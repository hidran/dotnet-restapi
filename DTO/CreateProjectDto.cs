using System.ComponentModel.DataAnnotations;

namespace PmsApi.DTO;

public class CreateProjectDto
{
    [Required(ErrorMessage = "Project Name is required")]
    public string ProjectName { get; set; } = String.Empty;

    public string? Description { get; set; }
    [Required(ErrorMessage = "Start date is required")]
    public DateOnly StartDate { get; set; }
    [Required(ErrorMessage = "End date is required")]

    public DateOnly EndDate { get; set; }
    [Required(ErrorMessage = "Category id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category must be greater or equal than 1")]

    public int CategoryId { get; set; }
    [Required(ErrorMessage = "Manager id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Manager must be greater or equal than 1")]

    public int ManagerId { get; set; }

}