

using AutoMapper;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Utilities;
using Microsoft.EntityFrameworkCore;
using PmsApi.Models;


namespace PmsApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly PmsContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextHelper _userContextHelper;

        public ProjectService(PmsContext context, IMapper mapper, IUserContextHelper userContextHelper)
        {
            _context = context;
            _mapper = mapper;
            _userContextHelper = userContextHelper;
        }
        public async Task<ProjectWithTasksDto?> GetProjectTasksAsync(
            int projectId,
        string userId,
         bool isAdmin)
        {
            var projectsQuery = _context.Projects.AsQueryable();
            //.Include(p => p.Tasks).Where(p => p.ProjectId == projectId);
            if (!isAdmin)
            {
                projectsQuery = projectsQuery.Where(p => p.ManagerId == userId);
            }

            var project = await projectsQuery.Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project is null) return null;

            var projectsDto = _mapper.Map<ProjectWithTasksDto>(project);
            return projectsDto;

        }
        public async Task<IEnumerable<ProjectWithTasksDto>> GetProjectsAsync(string userId, bool isAdmin, string include = "")
        {
            var projectsQuery = _context.Projects.AsQueryable();
            if (!_userContextHelper.IsAdmin())
            {
                projectsQuery = projectsQuery.Where(p => p.ManagerId == userId);
            }


            if (include.Contains("tasks", StringComparison.OrdinalIgnoreCase))
            {
                projectsQuery = projectsQuery.Include(p => p.Tasks);
            }
            if (include.Contains("manager", StringComparison.OrdinalIgnoreCase))
            {
                projectsQuery = projectsQuery.Include(p => p.Manager);
            }

            if (include.Contains("category", StringComparison.OrdinalIgnoreCase))
            {
                projectsQuery = projectsQuery.Include(p => p.Category);

            }

            var projects = await projectsQuery.ToListAsync();
            var projectsDto = _mapper.Map<IEnumerable<ProjectWithTasksDto>>(projects);
            return projectsDto;
        }
        public async Task<ProjectWithTasksDto?> GetProjectAsync(int projectId, string userId, bool isAdmin, string include = "")
        {
            var projectsQuery = _context.Projects.AsQueryable();
            if (!_userContextHelper.IsAdmin())
            {
                projectsQuery = projectsQuery.Where(p => p.ManagerId == userId);
            }

            if (include.Contains("tasks", StringComparison.OrdinalIgnoreCase))
                projectsQuery = projectsQuery.Include(p => p.Tasks);
            if (include.Contains("manager", StringComparison.OrdinalIgnoreCase))
                projectsQuery = projectsQuery.Include(p => p.Manager);
            if (include.Contains("category", StringComparison.OrdinalIgnoreCase))
                projectsQuery = projectsQuery.Include(p => p.Category);

            var project = await projectsQuery.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project is null) return null;
            var projectDto = _mapper.Map<ProjectWithTasksDto>(project);
            return projectDto;
        }
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto projectDto, string userId, bool isAdmin)
        {
            if (!isAdmin)
            {
                projectDto.ManagerId = userId;
            }
            var project = _mapper.Map<Project>(projectDto);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            var newProjectDto = _mapper.Map<ProjectDto>(project);
            return newProjectDto;

        }
        public async Task<bool?> UpdateProjectAsync(int projectId, CreateProjectDto projectDto, string userId, bool isAdmin)
        {
            var project = await _context.Projects.FindAsync(projectId);


            if (project is null || (!isAdmin && project.ManagerId != userId))
            {
                return null;
            }


            _mapper.Map(projectDto, project);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool?> DeleteProjectAsync(int projectId, string userId, bool isAdmin)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null || (!isAdmin && project.ManagerId != userId))
            {
                return null;
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }


    }

}