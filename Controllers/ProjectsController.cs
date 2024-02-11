using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;
using PmsApi.Services;
using PmsApi.Utilities;

namespace PmsApi.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    private readonly IUserContextHelper _userContextHelper;

    public ProjectsController(PmsContext context, IMapper mapper,
     IUserContextHelper userContextHelper, IProjectService projectService
     )
    {
        _projectService = projectService;
        _context = context;
        _mapper = mapper;
        _userContextHelper = userContextHelper;
    }

    [HttpGet("{projectId}/tasks")]
    public async Task<ActionResult<IEnumerable<ProjectWithTasksDto>>> GetProjectTasks(int projectId)
    {

        GetUserCredentials(out string userId, out bool isAdmin);

        var projectTasks = await _projectService.GetProjectTasksAsync(projectId, userId, isAdmin);

        if (projectTasks is null)
        {
            return NotFound();
        }
        return Ok(projectTasks);
    }

    private void GetUserCredentials(out string userId, out bool isAdmin)
    {
        userId = _userContextHelper.GetUserId();
        isAdmin = _userContextHelper.IsAdmin();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectWithTasksDto>>> GetProjects([FromQuery] string include = "")
    {
        GetUserCredentials(out string userId, out bool isAdmin);

        var projectsDto = await _projectService.GetProjectsAsync(userId, isAdmin, include);
        return Ok(projectsDto);
    }


    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectWithTasksDto>> GetProject(int projectId, [FromQuery] string include = "")
    {
        GetUserCredentials(out string userId, out bool isAdmin);
        var projectDto = await _projectService.GetProjectAsync(projectId, userId, isAdmin, include);
        if (projectDto is null)
        {
            return NotFound($"Project with id {projectId} not found");
        }
        return Ok(projectDto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        GetUserCredentials(out string userId, out bool isAdmin);


        try
        {

            var newProjectDto = await _projectService.CreateProjectAsync(projectDto, userId, isAdmin);
            return CreatedAtAction(nameof(GetProject),
                new { projectId = newProjectDto.ProjectId }, newProjectDto);
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
        if (!ModelState.IsValid) return BadRequest(ModelState);
        GetUserCredentials(out string userId, out bool isAdmin);


        try
        {
            var result = await _projectService.UpdateProjectAsync(projectId, projectDto, userId, isAdmin);
            if (result is null)
            {
                return NotFound();
            }
            if (result is false)
            {
                return StatusCode(500, "An error has occurred");
            }

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
        GetUserCredentials(out string userId, out bool isAdmin);

        try
        {
            var result = await _projectService.DeleteProjectAsync(projectId, userId, isAdmin);
            if (result is null)
            {
                return NotFound();
            }


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