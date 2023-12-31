﻿


namespace PmsApi.Models;

public class Task
{
    public int TaskId { get; set; }

    public string Title { get; set; } = String.Empty;

    public string? Description { get; set; }

    public int PriorityId { get; set; }

    public Priority? Priority { get; set; }
    public int StatusId { get; set; }

    public Status? Status { get; set; }

    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public int ProjectId { get; set; }

    public string? AssignedUserId { get; set; }

    public User? AssignedUser { get; set; }

    public Project? Project { get; set; }

    public ICollection<TaskAttachment> TaskAttachments { get; set; } = new List<TaskAttachment>();
}
