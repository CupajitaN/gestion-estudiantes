using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class MateriaRepository : IMateriaRepository
    {
        private readonly AppDbContext _context;

        public MateriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Materia>> GetAllAsync()
        {
            return await _context.Materias.ToListAsync();
        }

        public async Task<Materia?> GetByIdAsync(int id)
        {
            return await _context.Materias.FindAsync(id);
        }

        public async Task<Materia> CreateAsync(Materia materia)
        {
            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();
            return materia;
        }

        public async Task<Materia> UpdateAsync(Materia materia)
        {
            _context.Materias.Update(materia);
            await _context.SaveChangesAsync();
            return materia;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return false;

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
