using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.UserSkill;
//using Asp.Versioning;

namespace SkillMasteryAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]



/*[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]*/
public class UserSkillsController : ControllerBase
{
    private readonly IUserSkillService _userskillService;

    public UserSkillsController(IUserSkillService userskillService)
    {
        _userskillService = userskillService;
    }

    [HttpGet]
    [HttpHead]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserSkillDTO>>> GetAllUserSkills()
    {
        var userskills = await _userskillService.GetAllUserSkillsAsync();
        return Ok(userskills);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserSkillDTO>> CreateUserSkill([FromBody] CreateUserSkillDTO createUserSkillDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdUserSkillDTO = await _userskillService.CreateUserSkillAsync(createUserSkillDTO);
            return CreatedAtAction(null, new { id = createdUserSkillDTO.Id }, createdUserSkillDTO);
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
    public async Task<ActionResult<UserSkillDTO>> DeleteUserSkill(int id)
    {
        var course = await _userskillService.DeleteUserSkillAsync(id);

        return Ok(course);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserSkillDTO>> EditUserSkill([FromBody] UserSkillDTO editUserSkillDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var editedUserSkillDTO = await _userskillService.EditUserSkillAsync(editUserSkillDTO);
            return Ok(editedUserSkillDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
