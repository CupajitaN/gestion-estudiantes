using Backend.Models;

namespace Backend.Interfaces
{
    public interface IProfesorService
    {
        Task<IEnumerable<Profesor>> GetAllAsync();
        Task<Profesor?> GetByIdAsync(int id);
        Task<Profesor> CreateAsync(Profesor profesor);
        Task<Profesor?> ActualizarAsync(int id, Profesor profesor);
        Task<bool> EliminarAsync(int id);
    }
}
