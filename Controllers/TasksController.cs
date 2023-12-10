using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Utilities;
using Task = PmsApi.Models.Task;
namespace PmsApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly PmsContext _context;
    public TasksController(PmsContext pmsContext, IMapper mapper)
    {
        _context = pmsContext;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskAllDto>>> GetTasks([FromQuery] string include = "")
    {
        var tasksQuery = QueryHelper.ApplyIncludes(_context.Tasks.AsQueryable(), include);
        var tasks = await tasksQuery.ToListAsync();
        var tasksDto = _mapper.Map<IEnumerable<TaskAllDto>>(tasks);
        return Ok(tasksDto);
    }
    [HttpGet("{taskId}")]
    public async Task<ActionResult<TaskAllDto>>
     GetTask(int taskId, [FromQuery] string include = "")
    {
        var tasksQuery = QueryHelper.ApplyIncludes(_context.Tasks.AsQueryable(), include);

        var task = await tasksQuery.FirstOrDefaultAsync(x => x.TaskId == taskId);
        var tasksDto = _mapper.Map<TaskAllDto>(task);
        return Ok(tasksDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var task = _mapper.Map<Task>(taskDto);
        task.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
        _context.Tasks.Add(task);
        try
        {
            await _context.SaveChangesAsync();
            var newTaskDto = _mapper.Map<TaskDto>(task);

            return CreatedAtAction(nameof(GetTask),
            new { TaskId = task.TaskId }, newTaskDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlConnector.MySqlException
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Task? task = await _context.Tasks.FindAsync(taskId);

        if (task is null)
        {
            return NotFound($"Project with ID {taskId} not found.");
        }

        _mapper.Map(taskDto, task);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlConnector.MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Task name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }

}
