using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace PmsApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;

    public UsersController(PmsContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
        return Ok(usersDto);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return NotFound();
        }
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userDto);

        _context.Users.Add(user);
        try
        {
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Email already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }
    [HttpPatch("{userId:int}")]
    public async Task<ActionResult> UpdateUser(
        [FromRoute] int userId, [FromBody] UpdateUserDto userDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User? user = await _context.Users.FindAsync(userId);

        if (user is null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        _mapper.Map(userDto, user);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Email already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        User? user = await _context.Users.FindAsync(userId);

        if (user is null)
        {
            return NotFound($"No User found with ID {userId}");
        }
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
      when (e.InnerException is MySqlException)
        {

            return BadRequest("User has other records, please delete assigned tasks");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }

    }
}

