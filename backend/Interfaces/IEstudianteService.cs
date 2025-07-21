using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IEstudianteService
    {
        Task<List<Estudiante>> GetAllAsync();
        Task<Estudiante> GetByIdAsync(int id);
        Task<Estudiante> CreateAsync(Estudiante estudiante);
        Task<bool> DeleteAsync(int id);
        Task InscribirMateriasAsync(int estudianteId, List<int> materiaIds);
        Task<bool> UpdateAsync(int id, Estudiante estudiante);
        Task<Estudiante?> GetByUsuarioIdAsync(int usuarioId);
        Task<List<Estudiante>> ObtenerEstudiantesPorMateria(int materiaId);
        Task<List<Estudiante>> ObtenerEstudiantesCompartiendoClase(int estudianteId);
    }
}
