using Backend.Interfaces;
using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ProfesorService : IProfesorService
    {
        private readonly AppDbContext _context;

        public ProfesorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesor>> GetAllAsync()
        {
            return await _context.Profesores.ToListAsync();
        }

        public async Task<Profesor?> GetByIdAsync(int id)
        {
            return await _context.Profesores.FindAsync(id);
        }

        public async Task<Profesor> CreateAsync(Profesor profesor)
        {
            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();
            return profesor;
        }

        public async Task<Profesor?> ActualizarAsync(int id, Profesor profesorActualizado)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null) return null;

            profesor.Nombre = profesorActualizado.Nombre;
            profesor.Apellidos = profesorActualizado.Apellidos;
            profesor.Correo = profesorActualizado.Correo;
            profesor.Jornada = profesorActualizado.Jornada;
            profesor.MateriasMax = profesorActualizado.MateriasMax;

            await _context.SaveChangesAsync();
            return profesor;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null) return false;

            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
