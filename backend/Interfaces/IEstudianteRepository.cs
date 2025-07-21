using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IEstudianteRepository
    {
        Task<List<Estudiante>> GetAllAsync();
        Task<Estudiante> GetByIdAsync(int id);
        Task<Estudiante> CreateAsync(Estudiante estudiante);
        Task<bool> UpdateAsync(int id, Estudiante estudiante);
        Task<bool> DeleteAsync(int id);
        Task<List<Estudiante>> ObtenerEstudiantesCompartiendoClase(int estudianteId);
    }
}
