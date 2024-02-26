using Microsoft.EntityFrameworkCore;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository

    {
        private readonly SkillMasteryContext _context;

        public UserRepository(SkillMasteryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_context.User.ToList());
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUserAsync(User user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> EditUserAsync(User user)
        {
            var editedUser = await _context.User.FindAsync(user.Id);
            _context.User.Entry(editedUser!).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return user;
        }

    
    }
}
