using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;

        public EstudianteController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estudiantes = await _estudianteService.GetAllAsync();
            return Ok(estudiantes);
        }

        public class InscripcionRequest
        {
            public List<int> MateriaIds { get; set; } = new();
        }

        [HttpPost("inscribirse")]
        [Authorize(Roles = "estudiante,admin")]
        public async Task<IActionResult> Inscribirse([FromBody] InscripcionRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("No se pudo identificar al estudiante.");

            var estudiante = await _estudianteService.GetByUsuarioIdAsync(int.Parse(userId));
            if (estudiante == null)
                return NotFound("Estudiante no encontrado.");

            try
            {
                await _estudianteService.InscribirMateriasAsync(estudiante.Id, request.MateriaIds);
                return Ok(new { mensaje = "Inscripción exitosa." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] Estudiante estudiante)
        {
            var creado = await _estudianteService.CreateAsync(estudiante);
            return CreatedAtAction(nameof(GetAll), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Estudiante estudiante)
        {
            var actualizado = await _estudianteService.UpdateAsync(id, estudiante);
            return actualizado == null ? NotFound() : Ok(actualizado);
        }

        [HttpGet("creditos")]
        [Authorize(Roles = "estudiante")]
        public async Task<IActionResult> ObtenerCreditos()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("No se pudo identificar al estudiante.");

            var usuarioId = int.Parse(userIdClaim);

            var estudiante = await _estudianteService.GetByUsuarioIdAsync(usuarioId);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado");

            return Ok(new
            {
                maximos = estudiante.CreditosMaximos,
                utilizados = estudiante.CreditosUtilizados
            });
        }

        [HttpGet("por-materia/{materiaId}")]
        [Authorize(Roles = "profesor")]
        public async Task<IActionResult> ObtenerEstudiantesPorMateria(int materiaId)
        {
            var estudiantes = await _estudianteService.ObtenerEstudiantesPorMateria(materiaId);
            return Ok(estudiantes);
        }


        [HttpGet("por-materia-detalle/{materiaId}")]
        [Authorize(Roles = "admin,estudiante")]
        public async Task<IActionResult> ObtenerEstudiantesPorMateriaDetalle(int materiaId)
        {
            var estudiantes = await _estudianteService.ObtenerEstudiantesPorMateria(materiaId);

            var resultado = estudiantes
                .Select(e => new { e.NombreCompleto, e.Correo })
                .ToList();

            return Ok(resultado);
        }

    }
}
