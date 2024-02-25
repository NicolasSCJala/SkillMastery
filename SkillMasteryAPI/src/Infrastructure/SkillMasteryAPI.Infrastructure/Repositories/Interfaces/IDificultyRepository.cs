using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Repositories.Interfaces
{
    public interface IDificultyRepository
    {
        Task<IEnumerable<Dificulty>> GetAllDificultysAsync();
        Task<Dificulty> CreateDificultyAsync(Dificulty dificulty);
        Task<Dificulty?> GetDificultyByIdAsync(int id);
        Task<Dificulty> DeleteDificultyAsync(Dificulty dificulty);
        Task<Dificulty> EditDificultyAsync(Dificulty dificulty);
    }
}
