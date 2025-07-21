using Backend.Models;
using Backend.Interfaces;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CrearUsuario([FromBody] Usuario request)
    {
        var usuario = await _usuarioService.CrearUsuarioAsync(
            request.Correo,
            request.ContrasenaHash,
            request.Rol,
            request.EstudianteId,
            request.ProfesorId
        );

        if (usuario == null)
        {
            return Conflict(new { mensaje = "El correo ya está registrado." });
        }

        return Ok(usuario);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<object>> GetUsuarios([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var usuarios = await _usuarioService.GetAllAsync();

        var usuariosDto = usuarios.Select(u => new UsuarioDTO
        {
            Id = u.Id,
            Correo = u.Correo,
            Rol = u.Rol,
            EstudianteId = u.EstudianteId,
            ProfesorId = u.ProfesorId
        });

        // Si no hay paginación, retorna todo
        if (page == null || pageSize == null)
            return Ok(new { data = usuariosDto, total = usuariosDto.Count() });

        int skip = (page.Value - 1) * pageSize.Value;
        var paginated = usuariosDto.Skip(skip).Take(pageSize.Value);

        return Ok(new { data = paginated, total = usuariosDto.Count() });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var eliminado = await _usuarioService.EliminarAsync(id);
        if (!eliminado)
        {
            return NotFound(new { mensaje = "Usuario no encontrado o no se pudo eliminar." });
        }

        return NoContent(); // 204
    }

    [HttpGet("{id}/profesor-id")]
    [Authorize(Roles = "profesor")]
    public async Task<IActionResult> ObtenerProfesorIdPorUsuario(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null || usuario.ProfesorId == null)
            return NotFound(new { mensaje = "No se encontró un profesor vinculado a este usuario" });

        return Ok(new { profesorId = usuario.ProfesorId });
    }
}
