using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Repositories.Interfaces
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        Task<Skill> CreateSkillAsync(Skill skill);
        Task<Skill?> GetSkillByIdAsync(int id);
        Task<Skill> DeleteSkillAsync(Skill skill);
        Task<Skill> EditSkillAsync(Skill skill);
    }
}
