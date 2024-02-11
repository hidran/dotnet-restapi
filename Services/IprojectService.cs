using PmsApi.DTO;

namespace PmsApi.Services
{
    public interface IProjectService
    {
        public Task<ProjectWithTasksDto?> GetProjectTasksAsync(int projectId, string userId, bool isAdmin);

        public Task<IEnumerable<ProjectWithTasksDto>> GetProjectsAsync(string userId, bool isAdmin, string include = "");
        public Task<ProjectWithTasksDto?> GetProjectAsync(int projectId, string userId, bool isAdmin, string include = "");
        public Task<ProjectDto> CreateProjectAsync(CreateProjectDto projectDto, string userId, bool isAdmin);
        public Task<bool?> UpdateProjectAsync(int projectId, CreateProjectDto projectDto, string userId, bool isAdmin);
        public Task<bool?> DeleteProjectAsync(int projectId, string userId, bool isAdmin);


    }
}