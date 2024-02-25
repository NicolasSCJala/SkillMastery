using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Application.DTOs.Skill;
//using Asp.Versioning;

namespace SkillMasteryAPI.Presentation.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
//[ApiVersion("1.0")]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    [HttpHead]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SkillDTO>>> GetAllSkills()
    {
        var skills = await _skillService.GetAllSkillsAsync();
        return Ok(skills);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SkillDTO>> CreateSkill([FromBody] CreateSkillDTO createSkillDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdSkillDTO = await _skillService.CreateSkillAsync(createSkillDTO);
            return CreatedAtAction(null, new { id = createdSkillDTO.Id }, createdSkillDTO);
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
    public async Task<ActionResult<SkillDTO>> DeleteSkill(int id)
    {
        var course = await _skillService.DeleteSkillAsync(id);

        return Ok(course);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SkillDTO>> EditSkill([FromBody] SkillDTO editSkillDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var editedSkillDTO = await _skillService.EditSkillAsync(editSkillDTO);
            return Ok(editedSkillDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
