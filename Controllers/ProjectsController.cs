using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.DTO;

namespace Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly PmsContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(PmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectWithTasksDto>>> GetProjects([FromQuery] string include = "")
        {
            var projectsQuery = _context.Projects.AsQueryable();
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
            return Ok(projectsDto);
        }
    }
}