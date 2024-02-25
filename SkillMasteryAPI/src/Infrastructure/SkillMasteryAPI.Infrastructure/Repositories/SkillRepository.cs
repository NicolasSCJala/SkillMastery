
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;


namespace SkillMasteryAPI.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository

    {
        private readonly SkillMasteryContext _context;

        public SkillRepository(SkillMasteryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await Task.FromResult(_context.Skill.ToList());
        }

        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            await _context.Skill.AddAsync(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task<Skill> DeleteSkillAsync(Skill skill)
        {
            _context.Skill.Remove(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            return await _context.Skill.FindAsync(id);
        }

        public async Task<Skill> EditSkillAsync(Skill skill)
        {
            var editedSkill = await _context.Skill.FindAsync(skill.Id);
            _context.Skill.Entry(editedSkill!).CurrentValues.SetValues(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

       
    }
}
