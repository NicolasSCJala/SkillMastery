using SkillMasteryAPI.Application.DTOs.Skill;

namespace SkillMasteryAPI.Application.Services.Interfaces;

public interface ISkillService
{
    Task<IEnumerable<SkillDTO>> GetAllSkillsAsync();
    Task<SkillDTO> CreateSkillAsync(CreateSkillDTO createSkillDTO);
    Task<SkillDTO?> DeleteSkillAsync(int id);
    Task<SkillDTO> EditSkillAsync(SkillDTO goalDTO);

}
