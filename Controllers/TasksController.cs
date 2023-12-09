using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.DTO;

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
        var tasksQuery = _context.Tasks.AsQueryable();

        if (include.Contains("user", StringComparison.OrdinalIgnoreCase))
        {
            tasksQuery = tasksQuery.Include(t => t.AssignedUser);
        }
        if (include.Contains("project", StringComparison.OrdinalIgnoreCase))
        {
            tasksQuery = tasksQuery.Include(t => t.Project);
        }
        if (include.Contains("attachments", StringComparison.OrdinalIgnoreCase))
        {
            tasksQuery = tasksQuery.Include(t => t.TaskAttachments);
        }
        var tasks = await tasksQuery.ToListAsync();
        var tasksDto = _mapper.Map<IEnumerable<TaskAllDto>>(tasks);
        return Ok(tasksDto);
    }


}
