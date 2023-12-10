using System.ComponentModel.DataAnnotations;

namespace PmsApi.DTO;

public class CreateTaskDto
{

    [Required(ErrorMessage = "Task Title is required")]
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    [Required(ErrorMessage = "PriorityId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Priority must be greater or equal than 1")]

    public int PriorityId { get; set; }

    [Required(ErrorMessage = "StatusId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Status must be greater or equal than 1")]

    public int StatusId { get; set; }
    [Required(ErrorMessage = "Due Date is required")]

    public DateOnly DueDate { get; set; }
    [Required(ErrorMessage = "Project Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Project must be greater or equal than 1")]

    public int ProjectId { get; set; }
    [Required(ErrorMessage = "Assigned user is required")]
    [Range(1, int.MaxValue, ErrorMessage = "User must be greater or equal than 1")]

    public int AssignedUserId { get; set; }
}
