using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Repositories.Interfaces
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetAllGoalsAsync();
        Task<Goal> CreateGoalAsync(Goal goal);
        Task<Goal?> GetGoalByIdAsync(int id);
        Task<Goal> DeleteGoalAsync(Goal goal);
        Task<Goal> EditGoalAsync(Goal goal);
    }
}
