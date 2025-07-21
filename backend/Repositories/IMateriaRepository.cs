using Backend.Models;

namespace Backend.Interfaces
{
    public interface IMateriaRepository
    {
        Task<List<Materia>> GetAllAsync();
        Task<Materia?> GetByIdAsync(int id);
        Task<Materia> CreateAsync(Materia materia);
        Task<Materia> UpdateAsync(Materia materia);
        Task<bool> DeleteAsync(int id);
    }
}
