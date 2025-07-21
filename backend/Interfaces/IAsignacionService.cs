using Backend.DTOs;
using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAsignacionService
{
    Task<(List<ProfesorMateriaDto> Data, int Total)> ObtenerTodasAsync(int page = 1, int pageSize = 10);

    Task<IEnumerable<ProfesorMateriaDto>> ObtenerPorProfesorAsync(int profesorId);
    Task AsignarMateriaAProfesor(int profesorId, int materiaId);
    Task ActualizarAsignacion(int id, int profesorId, int materiaId);
    Task EliminarAsignacion(int id);
}