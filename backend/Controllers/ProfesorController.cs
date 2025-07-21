using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _profesorService;

        public ProfesorController(IProfesorService profesorService)
        {
            _profesorService = profesorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profesores = await _profesorService.GetAllAsync();
            return Ok(profesores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesor = await _profesorService.GetByIdAsync(id);
            return profesor == null ? NotFound() : Ok(profesor);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] Profesor profesor)
        {
            var creado = await _profesorService.CreateAsync(profesor);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Profesor profesor)
        {
            var actualizado = await _profesorService.ActualizarAsync(id, profesor);
            return actualizado == null ? NotFound() : Ok(actualizado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _profesorService.EliminarAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        
    }
}