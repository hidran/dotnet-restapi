using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Utilities;

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


}
