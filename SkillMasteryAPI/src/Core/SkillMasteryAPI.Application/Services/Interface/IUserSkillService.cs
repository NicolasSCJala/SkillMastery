using SkillMasteryAPI.Application.DTOs.UserSkill;

namespace SkillMasteryAPI.Application.Services.Interfaces;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillDTO>> GetAllUserSkillsAsync();
    Task<UserSkillDTO> CreateUserSkillAsync(CreateUserSkillDTO createUserSkillDTO);
    Task<UserSkillDTO?> DeleteUserSkillAsync(int id);
    Task<UserSkillDTO> EditUserSkillAsync(UserSkillDTO goalDTO);

}
