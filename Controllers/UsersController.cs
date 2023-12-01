using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.Models;

namespace PmsApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly PmsContext _context;

    public UsersController(PmsContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}

