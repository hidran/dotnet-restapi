using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Utilities;
using Task = PmsApi.Models.Task;

namespace PmsApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    private readonly IUserContextHelper _userContextHelper;

    public TasksController(PmsContext pmsContext, IMapper mapper, IUserContextHelper userContextHelper)
    {
        _context = pmsContext;
        _mapper = mapper;
        _userContextHelper = userContextHelper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskAllDto>>> GetTasks([FromQuery] string include = "")
    {
        var tasksQuery = QueryHelper.ApplyIncludes(_context.Tasks.AsQueryable(), include);
        if (!_userContextHelper.IsAdmin())
            tasksQuery = tasksQuery.Where(t => t.AssignedUserId == _userContextHelper.GetUserId());

        var tasks = await tasksQuery.ToListAsync();
        var tasksDto = _mapper.Map<IEnumerable<TaskAllDto>>(tasks);
        return Ok(tasksDto);
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<TaskAllDto>>
        GetTask(int taskId, [FromQuery] string include = "")
    {
        var tasksQuery = QueryHelper.ApplyIncludes(_context.Tasks.AsQueryable(), include);
        if (!_userContextHelper.IsAdmin())
            tasksQuery = tasksQuery.Where(t => t.AssignedUserId == _userContextHelper.GetUserId());
        var task = await tasksQuery.FirstOrDefaultAsync(x => x.TaskId == taskId);
        var tasksDto = _mapper.Map<TaskAllDto>(task);
        return Ok(tasksDto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!_userContextHelper.IsAdmin()) taskDto.AssignedUserId = _userContextHelper.GetUserId();
        var task = _mapper.Map<Task>(taskDto);
        task.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        _context.Tasks.Add(task);
        try
        {
            await _context.SaveChangesAsync();
            var newTaskDto = _mapper.Map<TaskDto>(task);

            return CreatedAtAction(nameof(GetTask),
                new { task.TaskId }, newTaskDto);
        }
        catch (DbUpdateException e)
            when (e.InnerException is MySqlException
                      mySqlException && mySqlException.Number == 1062)
        {
            return BadRequest("Task name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }

    [HttpPut("{taskId:int}")]
    public async Task<ActionResult> UpdateTask(
        [FromRoute] int taskId, [FromBody] CreateTaskDto taskDto
    )
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var task = await _context.Tasks.FindAsync(taskId);

        if (task is null) return NotFound($"Project with ID {taskId} not found.");

        if (!_userContextHelper.IsAdmin() && task.AssignedUserId != _userContextHelper.GetUserId())
            return Unauthorized();
        _mapper.Map(taskDto, task);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
            when (e.InnerException is MySqlException
                      mySqlException && mySqlException.Number == 1062)
        {
            return BadRequest("Task name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{taskId:int}")]
    public async Task<ActionResult> DeleteTask(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);

        if (task is null) return NotFound($"No Task found with ID {taskId}");
        if (!_userContextHelper.IsAdmin() && task.AssignedUserId != _userContextHelper.GetUserId())
            return Unauthorized();
        try
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
            when (e.InnerException is MySqlException)
        {
            return BadRequest("Task has other records, please delete assigned attachments");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpGet("{taskId}/attachments")]
    public async Task<ActionResult<IEnumerable<AttachmentWithTaskDto>>>
        GetTaskAttachments(int taskId)
    {
        var task = await _context.Tasks.Include(t => t.TaskAttachments).Where(t => t.TaskId == taskId)
            .FirstOrDefaultAsync();
        if (task is null) return NotFound($"No Task found with ID {taskId}");
        
        if (!_userContextHelper.IsAdmin() && task.AssignedUserId != _userContextHelper.GetUserId())
            return Unauthorized();
        var taskAttachments = task.TaskAttachments;
        var taskAttachmentsDto = _mapper.Map<IEnumerable<AttachmentWithTaskDto>>(taskAttachments);
        return Ok(taskAttachmentsDto);
    }
}