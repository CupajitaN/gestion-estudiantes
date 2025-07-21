using Backend.Interfaces; 
using Backend.Models;
using Backend.Repositories;
using Backend.Data;
using Microsoft.EntityFrameworkCore;


namespace Backend.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _repo;
        private readonly AppDbContext _context;

        public EstudianteService(IEstudianteRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<Estudiante>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Estudiante> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Estudiante> CreateAsync(Estudiante estudiante)
        {
            return await _repo.CreateAsync(estudiante);
        }

        public async Task<bool> UpdateAsync(int id, Estudiante estudiante)
        {
            return await _repo.UpdateAsync(id, estudiante);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task InscribirMateriasAsync(int estudianteId, List<int> materiaIds)
        {
            if (materiaIds.Count != 3)
                throw new Exception("El estudiante debe seleccionar exactamente 3 materias.");

            var materias = await _context.Materias
                .Where(m => materiaIds.Contains(m.Id))
                .Include(m => m.ProfesorMaterias)
                    .ThenInclude(pm => pm.Profesor)
                .ToListAsync();

            var profesores = materias
                .SelectMany(m => m.ProfesorMaterias)
                .Select(pm => pm.ProfesorId)
                .Distinct();

            if (profesores.Count() < materias.Count)
                throw new Exception("No puedes seleccionar materias con el mismo profesor.");

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteMaterias)
                .FirstOrDefaultAsync(e => e.Id == estudianteId);

            if (estudiante == null)
                throw new Exception("Estudiante no encontrado.");

            // Validar que no exceda los créditos máximos
            int creditosSeleccionados = materias.Sum(m => m.Creditos);

            if (creditosSeleccionados > estudiante.CreditosMaximos)
                throw new Exception($"No puedes inscribir más de {estudiante.CreditosMaximos} créditos.");

            // Limpiar materias anteriores si es necesario
            estudiante.EstudianteMaterias.Clear();

            // Agregar las nuevas materias
            estudiante.EstudianteMaterias = materias.Select(m => new EstudianteMateria
            {
                EstudianteId = estudiante.Id,
                MateriaId = m.Id
            }).ToList();

            // Actualizar créditos utilizados
            estudiante.CreditosUtilizados = creditosSeleccionados;

            await _context.SaveChangesAsync();
        }


        public async Task<Estudiante?> GetByUsuarioIdAsync(int usuarioId)
        {
            var estudiante = await _context.Estudiantes
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

            return estudiante;
        }


        public async Task<List<Estudiante>> ObtenerEstudiantesPorMateria(int materiaId)
        {
            return await _context.EstudianteMaterias
                .Where(em => em.MateriaId == materiaId && em.Estudiante != null)
                .Include(em => em.Estudiante)
                .Select(em => em.Estudiante!)
                .ToListAsync();
        }


        public async Task<List<Estudiante>> ObtenerEstudiantesCompartiendoClase(int estudianteId)
        {
            return await _repo.ObtenerEstudiantesCompartiendoClase(estudianteId);
        }

    }
}