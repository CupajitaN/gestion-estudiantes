using Backend.Interfaces;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AsignacionController : ControllerBase
    {
        private readonly IAsignacionService _asignacionService;

        public AsignacionController(IAsignacionService asignacionService)
        {
            _asignacionService = asignacionService;
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodas([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (data, total) = await _asignacionService.ObtenerTodasAsync(page, pageSize);
            return Ok(new { data, total });
        }

        [HttpGet("profesor/{profesorId}")]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> ObtenerPorProfesor(int profesorId)
        {
            var asignaciones = await _asignacionService.ObtenerPorProfesorAsync(profesorId);
            return Ok(asignaciones);
        }

        [HttpPost("asignar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Asignar([FromQuery] int profesorId, [FromQuery] int materiaId)
        {
            try
            {
                await _asignacionService.AsignarMateriaAProfesor(profesorId, materiaId);
                return Ok(new { mensaje = "Materia asignada con éxito." });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
        
        [HttpPut("asignaciones/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AsignacionUpdateDto dto)
        {
            try
            {
                await _asignacionService.ActualizarAsignacion(id, dto.ProfesorId, dto.MateriaId);
                return Ok(new { mensaje = "Asignación actualizada con éxito." });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpDelete("asignaciones/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _asignacionService.EliminarAsignacion(id);
                return Ok(new { mensaje = "Asignación eliminada con éxito." });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
