using PmsApi.DTO;

namespace PmsApi.Services
{
    public interface IProjectService
    {
        Task<ProjectWithTasksDto?> GetProjectTasksAsync(int projectId, string userId, bool isAdmin);
        // Task<IEnumerable<ProjectWithTasksDto>?> GetProjects(int projectId, string userId, bool isAdmin);

    }
}