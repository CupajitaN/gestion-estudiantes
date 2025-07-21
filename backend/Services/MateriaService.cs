using Backend.Interfaces;
using Backend.Models;
using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;


namespace Backend.Services
{
    public class MateriaService : IMateriaService
    {
        private readonly IMateriaRepository _repo;
        private readonly AppDbContext _context;

        public MateriaService(IMateriaRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<object> ObtenerMateriasAsync(int? page, int? pageSize)
        {
            var query = _context.Materias.AsQueryable();

            var total = await query.CountAsync();

            if (page != null && pageSize != null)
            {
                var skip = (page.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            var materias = await query.ToListAsync();

            return new
            {
                data = materias,
                total
            };
        }

        public async Task<Materia?> ObtenerMateriaPorIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }


        public async Task<Materia> CrearMateriaAsync(Materia materia)
        {
            return await _repo.CreateAsync(materia);
        }

        public async Task<Materia> ActualizarMateriaAsync(int id, Materia materiaActualizada)
        {
            var materia = await _repo.GetByIdAsync(id);
            if (materia == null) throw new Exception("Materia no encontrada");

            materia.Nombre = materiaActualizada.Nombre;
            materia.Descripcion = materiaActualizada.Descripcion;
            materia.Creditos = materiaActualizada.Creditos;

            return await _repo.UpdateAsync(materia);
        }

        public async Task<bool> EliminarMateriaAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<object> ObtenerMateriasPorEstudiante(int estudianteId, int? page, int? pageSize)
        {
            var query = _context.EstudianteMaterias
                .Where(em => em.EstudianteId == estudianteId)
                .Include(em => em.Materia)
                    .ThenInclude(m => m.ProfesorMaterias)
                        .ThenInclude(pm => pm.Profesor)
                .Select(em => new MateriaDTO
                {
                    Id = em.Materia.Id,
                    Nombre = em.Materia.Nombre,
                    Descripcion = em.Materia.Descripcion,
                    Profesor = em.Materia.ProfesorMaterias
                                .Select(pm => pm.Profesor.Nombre + " " + pm.Profesor.Apellidos)
                                .FirstOrDefault()
                });

            var total = await query.CountAsync();

            if (page != null && pageSize != null)
            {
                var skip = (page.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            var materias = await query.ToListAsync();

            return new
            {
                data = materias,
                total
            };
        }

        public async Task<object> ObtenerMateriasDeTodosLosEstudiantes(int? page, int? pageSize)
        {
            var estudiantes = await _context.Estudiantes
                .Include(e => e.EstudianteMaterias)
                    .ThenInclude(em => em.Materia)
                        .ThenInclude(m => m.ProfesorMaterias)
                            .ThenInclude(pm => pm.Profesor)
                .ToListAsync();

            var resultadoCompleto = estudiantes.SelectMany(e =>
                e.EstudianteMaterias.Select(em => new
                {
                    EstudianteId = e.Id,
                    Estudiante = $"{e.Nombre} {e.Apellidos}",
                    MateriaId = em.Materia.Id,
                    Nombre = em.Materia.Nombre,
                    Descripcion = em.Materia.Descripcion,
                    Profesor = em.Materia.ProfesorMaterias
                        .Select(pm => pm.Profesor.Nombre + " " + pm.Profesor.Apellidos)
                        .FirstOrDefault()
                })
            ).ToList();

            if (page == null || pageSize == null)
            {
                return new
                {
                    data = resultadoCompleto,
                    total = resultadoCompleto.Count
                };
            }

            var skip = (page.Value - 1) * pageSize.Value;
            var dataPaginada = resultadoCompleto.Skip(skip).Take(pageSize.Value).ToList();

            return new
            {
                data = dataPaginada,
                total = resultadoCompleto.Count
            };
        }


        public async Task<object> ObtenerMateriasPorProfesor(int profesorId, int? page, int? pageSize)
        {
            var query = _context.ProfesorMaterias
                .Where(pm => pm.ProfesorId == profesorId)
                .Include(pm => pm.Materia)
                .Select(pm => new
                {
                    id = pm.Materia.Id,
                    nombre = pm.Materia.Nombre,
                    descripcion = pm.Materia.Descripcion
                });

            var total = await query.CountAsync();

            if (page != null && pageSize != null)
            {
                var skip = (page.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }

            var materias = await query.ToListAsync();

            return new
            {
                data = materias,
                total
            };
        }

        
        public async Task<int?> ObtenerEstudianteIdDesdeUsuario(int userId)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == userId)
                .Select(u => u.EstudianteId)
                .FirstOrDefaultAsync();

            return usuario;
        }
    }
}
