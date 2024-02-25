using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.User;
//using Asp.Versioning;

namespace SkillMasteryAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]



/*[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]*/
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [HttpHead]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO createUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdUserDTO = await _userService.CreateUserAsync(createUserDTO);
            return CreatedAtAction(null, new { id = createdUserDTO.Id }, createdUserDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDTO>> DeleteUser(int id)
    {
        var course = await _userService.DeleteUserAsync(id);

        return Ok(course);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDTO>> EditUser([FromBody] UserDTO editUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var editedUserDTO = await _userService.EditUserAsync(editUserDTO);
            return Ok(editedUserDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
