using Microsoft.EntityFrameworkCore;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure.Repositories
{
    public class GoalRepository : IGoalRepository

    {
        private readonly SkillMasteryContext _context;

        public GoalRepository(SkillMasteryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetAllGoalsAsync()
        {
            return await Task.FromResult(_context.Goal.ToList());
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            await _context.Goal.AddAsync(goal);
            await _context.SaveChangesAsync();
            return goal;
        }

        public async Task<Goal> DeleteGoalAsync(Goal goal)
        {
            _context.Goal.Remove(goal);
            await _context.SaveChangesAsync();

            return goal;
        }

        public async Task<Goal> GetGoalByIdAsync(int id)
        {
            return await _context.Goal.FindAsync(id);
        }

        public async Task<Goal> EditGoalAsync(Goal goal)
        {
            var editedGoal = await _context.Goal.FindAsync(goal.Id);
            _context.Goal.Entry(editedGoal!).CurrentValues.SetValues(goal);
            await _context.SaveChangesAsync();
            return goal;
        }


    }
}
