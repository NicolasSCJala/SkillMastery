using Microsoft.EntityFrameworkCore;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Infrastructure.Data;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;

namespace SkillMasteryAPI.Infrastructure.Repositories
{
    public class DificultyRepository : IDificultyRepository

    {
        private readonly SkillMasteryContext _context;

        public DificultyRepository(SkillMasteryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dificulty>> GetAllDificultysAsync()
        {
            return await Task.FromResult(_context.Dificulty.ToList());
        }

        public async Task<Dificulty> CreateDificultyAsync(Dificulty dificulty)
        {
            await _context.Dificulty.AddAsync(dificulty);
            await _context.SaveChangesAsync();
            return dificulty;
        }

        public async Task<Dificulty> DeleteDificultyAsync(Dificulty dificulty)
        {
            _context.Dificulty.Remove(dificulty);
            await _context.SaveChangesAsync();

            return dificulty;
        }

        public async Task<Dificulty> GetDificultyByIdAsync(int id)
        {
            return await _context.Dificulty.FindAsync(id);
        }

        public async Task<Dificulty> EditDificultyAsync(Dificulty dificulty)
        {
            var editedDificulty = await _context.Dificulty.FindAsync(dificulty.Id);
            _context.Dificulty.Entry(editedDificulty!).CurrentValues.SetValues(dificulty);
            await _context.SaveChangesAsync();
            return dificulty;
        }


    }
}
