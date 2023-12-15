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
[Route("api/statuses")]
public class StatusController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    public StatusController(PmsContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetPriorities()
    {

        var statuses = await _context.Statuses.ToListAsync();
        var statusesDto = _mapper.Map<IEnumerable<StatusDto>>(statuses);
        return Ok(statusesDto);
    }



    [HttpGet("{statusId}")]
    public async Task<ActionResult<StatusDto>> GetStatus(int statusId)
    {

        Status? status = await _context.Statuses.FirstOrDefaultAsync(p => p.StatusId == statusId);
        if (status is null)
        {
            return NotFound();
        }
        var statusDto = _mapper.Map<StatusDto>(status);
        return Ok(statusDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateStatus([FromBody] CreateStatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var status = _mapper.Map<Status>(statusDto);

        _context.Statuses.Add(status);
        try
        {
            await _context.SaveChangesAsync();
            var newStatusDto = _mapper.Map<StatusDto>(status);

            return CreatedAtAction(nameof(GetStatus),
            new { statusId = status.StatusId }, newStatusDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("status name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }
    [HttpPut("{statusId:int}")]
    public async Task<ActionResult> UpdateStatus(
        [FromRoute] int statusId, [FromBody] CreateStatusDto statusDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Status? status = await _context.Statuses.FindAsync(statusId);

        if (status is null)
        {
            return NotFound($"Status with ID {statusId} not found.");
        }

        _mapper.Map(statusDto, status);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Status name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{statusId:int}")]
    public async Task<ActionResult> DeleteStatus(int statusId)
    {
        Status? status = await _context.Statuses.FindAsync(statusId);

        if (status is null)
        {
            return NotFound($"No status found with ID {statusId}");
        }
        try
        {
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
      when (e.InnerException is MySqlException)
        {

            return BadRequest("Status has other records, please delete assigned tasks");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }

    }
}
