

using AutoMapper;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Utilities;
using Microsoft.EntityFrameworkCore;


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
    }

}