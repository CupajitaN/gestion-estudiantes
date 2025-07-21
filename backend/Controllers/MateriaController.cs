using Backend.Interfaces;
using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MateriaController : ControllerBase
    {
        private readonly IMateriaService _materiaService;

        public MateriaController(IMateriaService materiaService)
        {
            _materiaService = materiaService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] Materia materia)
        {
            var creada = await _materiaService.CrearMateriaAsync(materia);
            return CreatedAtAction(nameof(Get), new { id = creada.Id }, creada);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(int id, [FromBody] Materia materia)
        {
            try
            {
                var actualizada = await _materiaService.ActualizarMateriaAsync(id, materia);
                return Ok(actualizada);
            }
            catch (Exception e)
            {
                return NotFound(new { mensaje = e.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminada = await _materiaService.EliminarMateriaAsync(id);
            return eliminada ? NoContent() : NotFound();
        }

        [HttpGet("estudiante")]
        public async Task<IActionResult> ObtenerMateriasDelEstudiante([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("No se pudo identificar al estudiante.");

            var userId = int.Parse(userIdClaim);

            var estudianteId = await _materiaService.ObtenerEstudianteIdDesdeUsuario(userId);

            if (estudianteId == null)
                return NotFound("El usuario no tiene un estudiante asociado.");

            var materias = await _materiaService.ObtenerMateriasPorEstudiante(estudianteId.Value, page, pageSize);
            return Ok(materias);
        }

        [HttpGet("estudiantes")]
        public async Task<IActionResult> GetMateriasDeTodosLosEstudiantes([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var resultado = await _materiaService.ObtenerMateriasDeTodosLosEstudiantes(page, pageSize);
            return Ok(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var resultado = await _materiaService.ObtenerMateriasAsync(page, pageSize);
            return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var materia = await _materiaService.ObtenerMateriaPorIdAsync(id);
            return materia == null ? NotFound() : Ok(materia);
        }

        [HttpGet("profesor/{profesorId}")]
        public async Task<IActionResult> ObtenerMateriasPorProfesor(int profesorId,[FromQuery] int? page,[FromQuery] int? pageSize)
        {
            var materias = await _materiaService.ObtenerMateriasPorProfesor(profesorId, page, pageSize);
            return Ok(materias);
        }
    }
}