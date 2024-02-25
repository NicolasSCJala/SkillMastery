using SkillMasteryAPI.Application.DTOs.Goal;

namespace SkillMasteryAPI.Application.Services.Interfaces;

public interface IGoalService
{
    Task<IEnumerable<GoalDTO>> GetAllGoalsAsync();
    Task<GoalDTO> CreateGoalAsync(CreateGoalDTO createGoalDTO);
    Task<GoalDTO?> DeleteGoalAsync(int id);
    Task<GoalDTO> EditGoalAsync(GoalDTO goalDTO);

}
