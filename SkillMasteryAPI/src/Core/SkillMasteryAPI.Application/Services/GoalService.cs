using MapsterMapper;
using SkillMasteryAPI.Application.DTOs.Goal;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Application.CrossCutting;

namespace SkillMasteryAPI.Application.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;
    private readonly IMapper _mapper;

    public GoalService(IGoalRepository goalRepository, IMapper mapper)
    {
        _goalRepository = goalRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GoalDTO>> GetAllGoalsAsync()
    {
        var goals = await _goalRepository.GetAllGoalsAsync();
        return _mapper.Map<IEnumerable<GoalDTO>>(goals);
    }

    public async Task<GoalDTO> CreateGoalAsync(CreateGoalDTO createGoalDTO)
    {
        var goal = _mapper.Map<Goal>(createGoalDTO);
        var createdGoal = await _goalRepository.CreateGoalAsync(goal);
        return _mapper.Map<GoalDTO>(createdGoal);
    }

    async public Task<GoalDTO?> DeleteGoalAsync(int id)
    {
        var goal = await _goalRepository.GetGoalByIdAsync(id);
        NotFoundException.ThrowIfNull(goal, $"Goal with id {id} not found");


        var deletedGoal = await _goalRepository.DeleteGoalAsync(goal!);

        return _mapper.Map<GoalDTO>(deletedGoal);
    }

    public async Task<GoalDTO> EditGoalAsync(GoalDTO editGoalDTO)
    {
        var goal = _mapper.Map<Goal>(editGoalDTO);
        var editedGoal = await _goalRepository.EditGoalAsync(goal);
        return _mapper.Map<GoalDTO>(editedGoal);
    }
}
