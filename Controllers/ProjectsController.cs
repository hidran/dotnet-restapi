using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace PmsApi.Controllers;

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

    [HttpGet("{projectId}/tasks")]
    public async Task<ActionResult<IEnumerable<ProjectWithTasksDto>>> GetProjectTasks(int projectId)
    {

        var project = await _context.Projects.Include(p => p.Tasks)
            .Where(p => p.ProjectId == projectId)

        .ToListAsync();
        if (project is null || project.Count == 0)
        {
            return NotFound();
        }
        var projectsDto = _mapper.Map<IEnumerable<ProjectWithTasksDto>>(project);
        return Ok(projectsDto);
    }
    [HttpGet()]
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


    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectWithTasksDto>> GetProject(int projectId, [FromQuery] string include = "")
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

        Project? project = await projectsQuery.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        if (project is null)
        {
            return NotFound();
        }
        var projectDto = _mapper.Map<ProjectWithTasksDto>(project);
        return Ok(projectDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = _mapper.Map<Project>(projectDto);

        _context.Projects.Add(project);
        try
        {
            await _context.SaveChangesAsync();
            var newProjectDto = _mapper.Map<ProjectDto>(project);

            return CreatedAtAction(nameof(GetProject),
            new { projectId = project.ProjectId }, newProjectDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("project name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }
    [HttpPut("{projectId:int}")]
    public async Task<ActionResult> UpdateProject(
        [FromRoute] int projectId, [FromBody] CreateProjectDto projectDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Project? project = await _context.Projects.FindAsync(projectId);

        if (project is null)
        {
            return NotFound($"Project with ID {projectId} not found.");
        }

        _mapper.Map(projectDto, project);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Project name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{projectId:int}")]
    public async Task<ActionResult> DeleteProject(int projectId)
    {
        Project? project = await _context.Projects.FindAsync(projectId);

        if (project is null)
        {
            return NotFound($"No project found with ID {projectId}");
        }
        try
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
      when (e.InnerException is MySqlException)
        {

            return BadRequest("Project has other records, please delete assigned tasks");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }

    }
}
