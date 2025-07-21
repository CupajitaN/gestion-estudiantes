using Backend.Models;
using Backend.DTOs;

namespace Backend.Interfaces
{
    public interface IMateriaService
    {
        Task<object> ObtenerMateriasAsync(int? page, int? pageSize);
        Task<Materia?> ObtenerMateriaPorIdAsync(int id);
        Task<Materia> CrearMateriaAsync(Materia materia);
        Task<Materia> ActualizarMateriaAsync(int id, Materia materia);
        Task<bool> EliminarMateriaAsync(int id);
        Task<object> ObtenerMateriasPorEstudiante(int estudianteId, int? page, int? pageSize);
        Task<object> ObtenerMateriasDeTodosLosEstudiantes(int? page, int? pageSize);
        Task<object> ObtenerMateriasPorProfesor(int profesorId, int? page, int? pageSize);
        Task<int?> ObtenerEstudianteIdDesdeUsuario(int userId);
    }
}
