using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace PmsApi.Controllers;

[ApiController]
[Route("api/users"), Authorize( Policy = "IsAdmin")]
public class UsersController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UsersController(PmsContext context, IMapper mapper, UserManager<User> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string include = "")
    {
       
        var usersQuery = _context.Users.AsQueryable();
       
        if (include.Contains("projects", StringComparison.OrdinalIgnoreCase))
        {
            usersQuery = usersQuery.Include(u => u.Projects);
        }
        if (include.Contains("tasks", StringComparison.OrdinalIgnoreCase))
        {
            usersQuery = usersQuery.Include(u => u.Tasks);
        }
        var users = await usersQuery.ToListAsync();
        var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
        return Ok(usersDto);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var isAdmin = HttpContext.User.IsInRole("Admin");
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

    [HttpPost, Authorize(Roles = "Admin, Editor")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userDto);

        try
        {
          var result = await _userManager.CreateAsync(user,"Test!123");
            
            if (!result.Succeeded)
            {
                return StatusCode(500, "An error has occurred creating user");
            }
            await _userManager.AddToRoleAsync(user, "User");
         
            var newUserDto = _mapper.Map<UserDto>(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, newUserDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Email already taken");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error has occurred: {ex.Message}");
        }
    }

    [HttpPatch("{userId}")]
    public async Task<ActionResult> UpdateUser(
        [FromRoute] string userId, [FromBody] UpdateUserDto userDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        User? user = await _userManager.FindByIdAsync(userId);

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


    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound($"No User found with ID {userId}");
        }
        try
        {
           await _userManager.DeleteAsync(user);
          
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

