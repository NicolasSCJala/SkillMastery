using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> DeleteUserAsync(User user);
        Task<User> EditUserAsync(User user);
    }
}
