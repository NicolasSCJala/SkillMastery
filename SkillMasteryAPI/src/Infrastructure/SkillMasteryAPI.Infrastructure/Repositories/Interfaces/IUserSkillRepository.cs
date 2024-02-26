using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Repositories.Interfaces
{
    public interface IUserSkillRepository
    {
        Task<IEnumerable<UserSkill>> GetAllUserSkillsAsync();
        Task<UserSkill> CreateUserSkillAsync(UserSkill userskill);
        Task<UserSkill?> GetUserSkillByIdAsync(int id);
        Task<UserSkill> DeleteUserSkillAsync(UserSkill userskill);
        Task<UserSkill> EditUserSkillAsync(UserSkill userskill);
    }
}
