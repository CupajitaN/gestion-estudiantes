using Backend.Interfaces;
using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Repositories
{
    public class EstudianteRepository : IEstudianteRepository
    {

        private readonly AppDbContext _context;

        public EstudianteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Estudiante>> GetAllAsync()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        public async Task<Estudiante> GetByIdAsync(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
                throw new Exception("Estudiante no encontrado");

            return estudiante;
        }

        public async Task<Estudiante> CreateAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task<bool> UpdateAsync(int id, Estudiante estudiante)
        {
            var existing = await _context.Estudiantes.FindAsync(id);
            if (existing == null) return false;

            existing.Nombre = estudiante.Nombre;
            existing.Apellidos = estudiante.Apellidos;
            existing.Correo = estudiante.Correo;
            // otros campos si los hay

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return false;

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<Estudiante>> ObtenerEstudiantesCompartiendoClase(int estudianteId)
        {
            var materiaIds = await _context.EstudianteMaterias
                .Where(em => em.EstudianteId == estudianteId)
                .Select(em => em.MateriaId)
                .ToListAsync();

            var estudiantes = await _context.EstudianteMaterias
                .Where(em => materiaIds.Contains(em.MateriaId) && em.EstudianteId != estudianteId)
                .Include(em => em.Estudiante)
                .Select(em => em.Estudiante!)
                .Distinct()
                .ToListAsync();

            return estudiantes;
        }
    }
}