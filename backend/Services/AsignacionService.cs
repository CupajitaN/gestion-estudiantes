using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AsignacionService : IAsignacionService
{
    private readonly AppDbContext _context;

    public AsignacionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ProfesorMateriaDto> Data, int Total)> ObtenerTodasAsync(int page = 1, int pageSize = 10)
    {
        var query = _context.ProfesorMaterias
            .Include(pm => pm.Materia)
            .Include(pm => pm.Profesor)
            .Select(pm => new ProfesorMateriaDto
            {
                Id = pm.Id,
                ProfesorId = pm.ProfesorId,
                MateriaId = pm.MateriaId,
                NombreMateria = pm.Materia.Nombre,
                NombreProfesor = pm.Profesor.Nombre + " " + pm.Profesor.Apellidos,
            });

        var total = await query.CountAsync();

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<IEnumerable<ProfesorMateriaDto>> ObtenerPorProfesorAsync(int profesorId)
    {
        return await _context.ProfesorMaterias
            .Include(pm => pm.Materia)
            .Include(pm => pm.Profesor)
            .Where(pm => pm.ProfesorId == profesorId)
            .Select(pm => new ProfesorMateriaDto
            {
                Id = pm.Id,
                ProfesorId = pm.ProfesorId,
                MateriaId = pm.MateriaId,
                NombreMateria = pm.Materia.Nombre,
                NombreProfesor = pm.Profesor.Nombre + " " + pm.Profesor.Apellidos,
            })
            .ToListAsync();
    }

    public async Task AsignarMateriaAProfesor(int profesorId, int materiaId)
    {
        var profesor = await _context.Profesores
            .Include(p => p.ProfesorMaterias)
            .FirstOrDefaultAsync(p => p.Id == profesorId);

        if (profesor == null)
            throw new Exception("Profesor no encontrado");

        if (profesor.ProfesorMaterias.Count >= 2)
            throw new Exception("Este profesor ya dicta 2 materias.");

        var materia = await _context.Materias.FindAsync(materiaId);
        if (materia == null)
            throw new Exception("Materia no encontrada");

        // Validar que no se repita la asignación
        bool yaAsignada = await _context.ProfesorMaterias
            .AnyAsync(pm => pm.ProfesorId == profesorId && pm.MateriaId == materiaId);

        if (yaAsignada)
            throw new Exception("Esta materia ya está asignada a este profesor.");

        
        // Verificar si la materia ya está asignada a un profesor
        var materiaYaAsignada = await _context.ProfesorMaterias
            .AnyAsync(pm => pm.MateriaId == materiaId);

        if (materiaYaAsignada)
        {
            throw new Exception("Esta materia ya está asignada a otro profesor.");
        }

        var asignacion = new ProfesorMateria
        {
            ProfesorId = profesorId,
            Profesor = profesor!,
            MateriaId = materiaId,
            Materia = materia!
        };

        _context.ProfesorMaterias.Add(asignacion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsignacion(int id, int profesorId, int materiaId)
    {
        var asignacion = await _context.ProfesorMaterias.FindAsync(id);
        if (asignacion == null) throw new Exception("Asignación no encontrada");

        asignacion.ProfesorId = profesorId;
        asignacion.MateriaId = materiaId;

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsignacion(int id)
    {
        var asignacion = await _context.ProfesorMaterias.FindAsync(id);
        if (asignacion == null) throw new Exception("Asignación no encontrada");

        _context.ProfesorMaterias.Remove(asignacion);
        await _context.SaveChangesAsync();
    }
}
