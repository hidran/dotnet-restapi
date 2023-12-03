using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace Controllers;

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
    // //[HttpPatch("{userId:int}")]
    // public async Task<ActionResult> UpdateUser(
    //     [FromRoute] int userId, [FromBody] UpdateUserDto userDto
    //     )
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     User? user = await _context.Users.FindAsync(userId);

    //     if (user is null)
    //     {
    //         return NotFound($"User with ID {userId} not found.");
    //     }

    //     _mapper.Map(userDto, user);
    //     try
    //     {
    //         await _context.SaveChangesAsync();

    //         return NoContent();
    //     }
    //     catch (DbUpdateException e)
    //     when (e.InnerException is MySqlException
    //      mySqlException && mySqlException.Number == 1062)
    //     {

    //         return BadRequest("Email already taken");
    //     }
    //     catch (Exception)
    //     {
    //         return StatusCode(500, "An error has occurred");
    //     }
    // }


    // // [HttpDelete("{userId:int}")]
    // public async Task<ActionResult> DeleteUser(int userId)
    // {
    //     User? user = await _context.Users.FindAsync(userId);

    //     if (user is null)
    //     {
    //         return NotFound($"No User found with ID {userId}");
    //     }
    //     try
    //     {
    //         _context.Users.Remove(user);
    //         await _context.SaveChangesAsync();
    //         return NoContent();
    //     }
    //     catch (DbUpdateException e)
    //   when (e.InnerException is MySqlException)
    //     {

    //         return BadRequest("User has other records, please delete assigned tasks");
    //     }
    //     catch (Exception)
    //     {
    //         return StatusCode(500, "An error has occurred");
    //     }

    // }
}
