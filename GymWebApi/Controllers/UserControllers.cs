using GymWebApi.Entities;
using GymWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(User user)
    {
        var created = await _userService.CreateUserAsync(user);
        if (!created) return BadRequest();
        return CreatedAtAction(nameof(GetUserById), new { id = user.ID }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User user)
    {
        if (id != user.ID) 
            return BadRequest();
        var updated = await _userService.UpdateUserAsync(user);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var deleted = await _userService.DeleteUserAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
