using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace Controllers;

[ApiController]
[Route("api/priorities")]
public class PrioritiesController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    public PrioritiesController(PmsContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PriorityDto>>> GetPriorities()
    {

        var priorities = await _context.Priorities.ToListAsync();
        var prioritiesDto = _mapper.Map<IEnumerable<PriorityDto>>(priorities);
        return Ok(prioritiesDto);
    }



    [HttpGet("{priorityId}")]
    public async Task<ActionResult<PriorityDto>> GetPriority(int priorityId)
    {

        Priority? priority = await _context.Priorities.FirstOrDefaultAsync(p => p.PriorityId == priorityId);
        if (priority is null)
        {
            return NotFound();
        }
        var priorityDto = _mapper.Map<PriorityDto>(priority);
        return Ok(priorityDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreatePriority([FromBody] CreatePriorityDto priorityDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var priority = _mapper.Map<Priority>(priorityDto);

        _context.Priorities.Add(priority);
        try
        {
            await _context.SaveChangesAsync();
            var newPriorityDto = _mapper.Map<PriorityDto>(priority);

            return CreatedAtAction(nameof(GetPriority),
            new { priorityId = priority.PriorityId }, newPriorityDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("priority name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }
    [HttpPut("{priorityId:int}")]
    public async Task<ActionResult> UpdatePriority(
        [FromRoute] int priorityId, [FromBody] CreatePriorityDto priorityDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Priority? priority = await _context.Priorities.FindAsync(priorityId);

        if (priority is null)
        {
            return NotFound($"Priority with ID {priorityId} not found.");
        }

        _mapper.Map(priorityDto, priority);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Priority name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{priorityId:int}")]
    public async Task<ActionResult> DeletePriority(int priorityId)
    {
        Priority? priority = await _context.Priorities.FindAsync(priorityId);

        if (priority is null)
        {
            return NotFound($"No priority found with ID {priorityId}");
        }
        try
        {
            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
      when (e.InnerException is MySqlException)
        {

            return BadRequest("Priority has other records, please delete assigned tasks");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }

    }
}
