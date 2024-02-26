using Microsoft.EntityFrameworkCore;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure.Repositories
{
    public class UserSkillRepository : IUserSkillRepository

    {
        private readonly SkillMasteryContext _context;

        public UserSkillRepository(SkillMasteryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserSkill>> GetAllUserSkillsAsync()
        {
            return await Task.FromResult(_context.UserSkill.ToList());
        }

        public async Task<UserSkill> CreateUserSkillAsync(UserSkill userskill)
        {
            await _context.UserSkill.AddAsync(userskill);
            await _context.SaveChangesAsync();
            return userskill;
        }

        public async Task<UserSkill> DeleteUserSkillAsync(UserSkill userskill)
        {
            _context.UserSkill.Remove(userskill);
            await _context.SaveChangesAsync();

            return userskill;
        }

        public async Task<UserSkill> GetUserSkillByIdAsync(int id)
        {
            return await _context.UserSkill.FindAsync(id);
        }

        public async Task<UserSkill> EditUserSkillAsync(UserSkill userskill)
        {
            var editedUserSkill = await _context.UserSkill.FindAsync(userskill.Id);
            _context.UserSkill.Entry(editedUserSkill!).CurrentValues.SetValues(userskill);
            await _context.SaveChangesAsync();
            return userskill;
        }


    }
}

